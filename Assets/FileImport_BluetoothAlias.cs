using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class FileImport_BluetoothAlias :MonoBehaviour{


    public UniqueIdOrAliasToStringChanelRegisterMono m_listOfAlias;
    public void ClearRegister()
    {
        m_listOfAlias.Clear();
        

    }



public void Load(params string[] filePath)
{
    for (int i = 0; i < filePath.Length; i++)
    {
        if (File.Exists(filePath[i]))
        {
            ImportFileText(File.ReadAllText(filePath[i]));
        }
    }
}


    public List<DeviceToStringChannel> m_deviceToChannel = new List<DeviceToStringChannel>();
private void ImportFileText(string text)
{
    string[] lines = text.Split('\n');
    TileLine tokens;
    for (int i = 0; i < lines.Length; i++)
    {
        char[] l = lines[i].Trim().ToCharArray();
        if (l.Length > 0 && (l[0] == '#' || (l[0] == '/' && l[1] == '/')))
            continue;
        tokens = new TileLine(lines[i]);


            if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(0), "☗Read", true, true))
            {
                m_deviceToChannel.Add(new DeviceToStringChannel(tokens.GetValue(1), tokens.GetValue(2)));
            }
            else if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(0), "☗Group", true, true))
            {
                string groupName = tokens.GetValue(1);
                List<string> s = new List<string>();
                for (int j = 2; j < tokens.GetCount(); j++)
                {
                    s.Add(tokens.GetValue(j));
                }
                m_listOfAlias.AddToList(groupName, s);
            }

            else if (tokens.GetCount() == 2)
            {
                m_onAliasFound.Invoke(tokens.GetValue(0), tokens.GetValue(1));
            }
            else if (tokens.GetCount() == 3)
            {
                if (int.TryParse(tokens.GetValue(2), out int result)) { 
                    m_onAliasIndexFound.Invoke(tokens.GetValue(0), tokens.GetValue(1),result);
                }
            }
    }

    }
    public AliasFoundEvent m_onAliasFound;
    public AliasIndexFoundEvent m_onAliasIndexFound;

    [System.Serializable]
    public class AliasFoundEvent : UnityEvent<string, string> { }
    [System.Serializable]
    public class AliasIndexFoundEvent : UnityEvent<string, string, int> { }

    [System.Serializable]
    public class DeviceToStringChannel
    {
        public string m_deviceNameAlias;
        public string m_channelNameId;

        public DeviceToStringChannel(string deviceNameAlias, string channelNameId)
        {
            m_deviceNameAlias = deviceNameAlias;
            m_channelNameId = channelNameId;
        }
    }
   
}