using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HIDObserveUserIntentLinkToCurrentDeviceMono : MonoBehaviour
{

    public HIDObserveUserIntentMono m_userIntent;
    public ListOfAllDeviceAsIdBoolFloatMono m_currentDeviceList;
    public List<HIDRef_DeviceButtonUniqueID> m_deviceButtonReference = new List<HIDRef_DeviceButtonUniqueID>();
    public List<HIDRef_DeviceAxisUniqueID> m_deviceAxisReference = new List<HIDRef_DeviceAxisUniqueID>();
    public Dictionary<string, List<HIDRef_DeviceButtonUniqueID>> m_buttonDico = new Dictionary<string, List<HIDRef_DeviceButtonUniqueID>>();
    public Dictionary<string, List<HIDRef_DeviceAxisUniqueID>> m_axisDico = new Dictionary<string, List<HIDRef_DeviceAxisUniqueID>>();


    [ContextMenu("Refresh")]
    public void Refresh() {

        m_buttonDico.Clear();
        m_axisDico.Clear();


        

        foreach (var item in m_userIntent.m_buttonDirectUniqueId)
        {
            string id = item.m_uniqueID;
            HIDButtonStatic.SplitUniqueId(item.m_uniqueID,
                out string path,
                out string button);

            HIDRef_DeviceButtonUniqueID buttonTrack = new HIDRef_DeviceButtonUniqueID();
            buttonTrack.m_uniqueID = id;
            buttonTrack.m_booleanName = item.m_booleanName;
            buttonTrack.m_buttonObserver = new Intent_ObserverHIDButton() {
                m_buttonName = button,
                m_trueIfPressed= item.m_buttonObserved.m_valueIsTrue
            };
            if (!m_buttonDico.ContainsKey(id))
                m_buttonDico.Add(id, new List<HIDRef_DeviceButtonUniqueID>());
            m_buttonDico[id].Add(buttonTrack);
        }
        foreach (var item in m_userIntent.m_axisDirectUniqueId)
        {
            string id = item.m_uniqueID;
            HIDButtonStatic.SplitUniqueId(item.m_uniqueID,
                out string path,
                out string button);

            HIDRef_DeviceAxisUniqueID buttonTrack = new HIDRef_DeviceAxisUniqueID();
            buttonTrack.m_uniqueID = id;
            buttonTrack.m_booleanName = item.m_booleanName;
            buttonTrack.m_axisObserver = new Intent_ObserverHIDAxis() {
                m_axisName = button,
                m_inverse = item.m_axisObserved.m_inverse,
                m_betweenMin = item.m_axisObserved.m_betweenMin,
                m_betweenMax = item.m_axisObserved.m_betweenMax
            };
            if (!m_axisDico.ContainsKey(id))
                m_axisDico.Add(id, new List<HIDRef_DeviceAxisUniqueID>());
            m_axisDico[id].Add(buttonTrack);
        }


        //c;
        foreach (var item in m_userIntent.m_searchPathDeviceButton)
        {
            string id = HIDButtonStatic.GetIDPathAndButtonName(
                in item.m_deviceToExactPathObserver,
                in item.m_buttonObserver.m_buttonName);

            HIDRef_DeviceButtonUniqueID buttonTrack = new HIDRef_DeviceButtonUniqueID();
            buttonTrack.m_uniqueID = id;
            buttonTrack.m_booleanName = item.m_booleanName;
            buttonTrack.m_buttonObserver = item.m_buttonObserver;
            if (!m_buttonDico.ContainsKey(id))
                m_buttonDico.Add(id, new List<HIDRef_DeviceButtonUniqueID>());
            m_buttonDico[id].Add(buttonTrack);
        }
        foreach (var item in m_userIntent.m_searchPathDeviceAxis)
        {
            string id = HIDButtonStatic.GetIDPathAndButtonName(
                in item.m_deviceToExactPathObserver,
                in item.m_axisObserver.m_axisName);

            HIDRef_DeviceAxisUniqueID buttonTrack = new HIDRef_DeviceAxisUniqueID();
            buttonTrack.m_uniqueID = id;
            buttonTrack.m_booleanName = item.m_booleanName;
            buttonTrack.m_axisObserver = item.m_axisObserver;
            if(!m_axisDico.ContainsKey(id))
                m_axisDico.Add(id,new List<HIDRef_DeviceAxisUniqueID>());
            m_axisDico[id].Add(buttonTrack);
        }

        //m_userIntent.m_searchPathDeviceAxis;
        //m_userIntent.m_searchAnyDeviceButton;
        //m_userIntent.m_searchAnyDeviceAxis;

        foreach (var item in m_userIntent.m_allButtonsFromPath)
        {
            m_currentDeviceList.GetDeviceInfoFromPath(item.m_devicePath, out bool found, out  DeviceSourceToRawValue device);
            if (!found)
                continue;
            foreach (var b in device.m_booleanValue)
            {
                string id = HIDButtonStatic.GetIDPathAndButtonName(
                in device.m_devicePath,
                in b.m_givenIdName );

                HIDRef_DeviceButtonUniqueID buttonTrack = new HIDRef_DeviceButtonUniqueID();
                buttonTrack.m_uniqueID = id;
                buttonTrack.m_booleanName = item.m_buttonsObserver.m_frontOfButtons+ b.m_givenIdName.Replace(" ", item.m_buttonsObserver.m_replaceSpaceWith);
                buttonTrack.m_buttonObserver = new Intent_ObserverHIDButton() {
                    m_buttonName = b.m_givenIdName,
                    m_trueIfPressed = true
                };
                if (!m_buttonDico.ContainsKey(id))
                    m_buttonDico.Add(id, new List<HIDRef_DeviceButtonUniqueID>());
                m_buttonDico[id].Add(buttonTrack);
            }
            
           
        }
        m_deviceButtonReference.Clear();
        m_deviceAxisReference.Clear();
        foreach (var item in m_buttonDico.Values)
        {

            m_deviceButtonReference.AddRange(item);
        }
        foreach (var item in m_axisDico.Values)
        {

            m_deviceAxisReference.AddRange(item);
        }
    }

    //private void GetDeviceToObserver(HIDSearchPattern_LookAroundWithIndex m_deviceToObserver, out DeviceSourceToRawValue[] devicesFound)
    //{
    //    throw new NotImplementedException();
    //}
}
