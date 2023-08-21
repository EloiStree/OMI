using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ListOfAllDeviceBooleanChangeListenerMono : MonoBehaviour
{
    public ListOfAllDeviceAsIdBoolFloatMono m_sourceObservered;

    public List<string> m_debugSpammerList = new List<string>();

    public Dictionary<string, AntiSpamBuffer> m_antiSpam = new Dictionary<string, AntiSpamBuffer>();
    public Dictionary<string, bool> m_permaBan = new Dictionary<string, bool>();
    public Dictionary<string, HIDButtonChangedReference> m_register = new Dictionary<string, HIDButtonChangedReference>();

    public HIDButtonChangedReferenceEvent m_onButtonChanged;
    public HIDButtonChangedReferenceEvent m_onButtonIsConsiderAsSpam;
    public class AntiSpamBuffer {
        public bool m_isSpammer;
        public int m_tempCount;

        public void DecreaseToken(int valueToRemove)
        {
            m_tempCount -= valueToRemove;
        }
    }

    public ulong m_received;
    public ulong m_relayed;
    public LastReceivedBoolChange m_lastReceived;

    public List<string> m_buttonUniqueId = new List<string>();

    //public History m_history= new History();

    [System.Serializable]
    public class History : Eloi.GenericClampHistory<LastReceivedBoolChange> { }

    private void Awake()
    {
        m_sourceObservered.m_onBooleanChanged += DebugBooleanChanged;
        InvokeRepeating("DecreaseToken", 0, 0.1f);
        InvokeRepeating("RefreshListOfButton", 0, 1);
    }

    private void OnDestroy()
    {

        m_sourceObservered.m_onBooleanChanged -= DebugBooleanChanged;
    }

    public void RefreshListOfButton() {

        m_buttonUniqueId = m_register.Keys.ToList() ;
    }

    private void DecreaseToken()
    {
        foreach (var item in m_antiSpam.Values)
        {
            item.DecreaseToken(5);
        }
    }

    public int m_maxToken = 100;
    private void DebugBooleanChanged(DeviceSourceToRawValue deviceInfo, DeviceSourceToRawValue.NamedBooleanValue booleanThatChanged, bool newValue)
    {
        if (deviceInfo == null || booleanThatChanged == null)
            return;
        m_received++;
        string id = booleanThatChanged.GetGUID();
        if (m_permaBan.ContainsKey(id))
            return;

        if (!m_antiSpam.ContainsKey(id))
            m_antiSpam.Add(id, new AntiSpamBuffer());
        AntiSpamBuffer spam = m_antiSpam[id];
        if (spam.m_isSpammer)
            return;

        spam.m_tempCount++;
        if (spam.m_tempCount > m_maxToken) {
            spam.m_isSpammer = true;
           // Debug.Log("SPAMMER !!! " + deviceInfo.m_devicePath + " " + booleanThatChanged.m_givenIdName);
            m_debugSpammerList.Add(deviceInfo.m_devicePath + " " + booleanThatChanged.m_givenIdName);
            m_permaBan.Add(id, true);
            m_onButtonIsConsiderAsSpam.Invoke(m_register[HIDButtonStatic.GetID(in deviceInfo, in booleanThatChanged)]);
        }
        m_relayed++;
        string uniqueId = HIDButtonStatic.GetID(in deviceInfo, in booleanThatChanged);
        if (!m_register.ContainsKey(uniqueId))
            m_register.Add(uniqueId ,new HIDButtonChangedReference(in deviceInfo, in booleanThatChanged));


        m_lastReceived.m_deviceInfo = deviceInfo;
        m_lastReceived.m_booleanThatChanged = booleanThatChanged;
        m_lastReceived.m_newValue = newValue;
       
        m_onButtonChanged.Invoke(m_register[uniqueId]);
    }
    [System.Serializable]
    public struct LastReceivedBoolChange
    {
        public DeviceSourceToRawValue m_deviceInfo;
        public DeviceSourceToRawValue.NamedBooleanValue m_booleanThatChanged;
        public bool m_newValue;

        public LastReceivedBoolChange(DeviceSourceToRawValue deviceInfo, DeviceSourceToRawValue.NamedBooleanValue booleanThatChanged, bool newValue)
        {
            m_deviceInfo = deviceInfo;
            m_booleanThatChanged = booleanThatChanged;
            m_newValue = newValue;
        }
    }
}

[System.Serializable]
public class HIDButtonChangedReferenceEvent : UnityEvent<HIDButtonChangedReference> { }

[System.Serializable]
public class HIDButtonChangedReference
{
    public string m_uniqueId;
    public DeviceSourceToRawValue m_deviceInfo;
    public DeviceSourceToRawValue.NamedBooleanValue m_booleanThatChanged;

    public HIDButtonChangedReference(in DeviceSourceToRawValue deviceInfo, in DeviceSourceToRawValue.NamedBooleanValue booleanThatChanged)
    {
        m_uniqueId = HIDButtonStatic.GetID(in deviceInfo, in booleanThatChanged);
        m_deviceInfo = deviceInfo;
        m_booleanThatChanged = booleanThatChanged;
    }
}



[System.Serializable]
public class HIDAxisChangedReferenceEvent : UnityEvent<HIDAxisChangedReference> { }

[System.Serializable]
public class HIDAxisChangedReference
{
    public string m_uniqueId;
    public DeviceSourceToRawValue m_deviceInfo;
    public DeviceSourceToRawValue.NamedFloatValue m_axisThatChanged;

    public HIDAxisChangedReference(in DeviceSourceToRawValue deviceInfo, in DeviceSourceToRawValue.NamedFloatValue axisThatChanged)
    {
        m_uniqueId = HIDButtonStatic.GetID(in deviceInfo, in axisThatChanged);
        m_deviceInfo = deviceInfo;
        m_axisThatChanged = axisThatChanged;
    }
}


public class HIDButtonStatic {
    public static string GetID(in DeviceSourceToRawValue device, in DeviceSourceToRawValue.NamedBooleanValue booleanThatChanged)
    {
        return device.m_devicePath + "|>" + booleanThatChanged.m_givenIdName;
    }
    public static string GetID(in DeviceSourceToRawValue device, in DeviceSourceToRawValue.NamedFloatValue booleanThatChanged)
    {
        return device.m_devicePath + "|>" + booleanThatChanged.m_givenIdName;
    }

    public static string GetIDPathAndButtonName(
        in string devicePath,in string buttonName)
    {
        return devicePath + "|>" + buttonName;
    }

    internal static void SplitUniqueId(string uniqueID, out string path, out string button)
    {
        path = "";
        button = "";
        string [] token = uniqueID.Split("|>");
        if (token.Length > 0)
            path = token[0];
        if (token.Length > 1)
            button = token[1];
    }
} 
