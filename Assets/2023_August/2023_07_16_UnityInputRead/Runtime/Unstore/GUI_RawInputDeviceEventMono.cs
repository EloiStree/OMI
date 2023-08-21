using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GUI_RawInputDeviceEventMono : MonoBehaviour
{

    public DeviceSourceToRawValue m_deviceSourceRawValue;

    public Eloi.PrimitiveUnityEvent_String m_onDisplayName;
    public Eloi.PrimitiveUnityEvent_String m_onProductName;
    public Eloi.PrimitiveUnityEvent_String m_onManifacturer;
    public Eloi.PrimitiveUnityEvent_String m_onInterfaceName;
    public Eloi.PrimitiveUnityEvent_String m_onCapacbilities;
    public Eloi.PrimitiveUnityEvent_String m_onDevicePath;
    public Eloi.PrimitiveUnityEvent_Int    m_onDeviceButtonCount;
    public Eloi.PrimitiveUnityEvent_Int    m_onDeviceAxisCount;

    public GUI_NamedBooleanWithIndexMono [] m_booleanGUI;
    public GUI_NamedAxisWithIndexMono [] m_axisGUI;
    public bool m_useJsonCapacityInLog=true;
    public Eloi.PrimitiveUnityEvent_String m_onCreateClipboardDebugger;
    public Eloi.PrimitiveUnityEvent_String m_onCreateLocalFileDebugger;




    public void CopyDeviceInfoInClipboard()
    {
        string log = CreateExportLog(m_useJsonCapacityInLog);
        m_onCreateClipboardDebugger.Invoke(log);

    }

    public string CreateExportLog( bool useJsonCapacityInLog =true)
    {
        return DeviceToLogUtility.CreateExportLog(m_deviceSourceRawValue, useJsonCapacityInLog);
    }

    public void CopyDeviceInfoInLocalFile()
    {
        string log = CreateExportLog();
        m_onCreateLocalFileDebugger.Invoke(log);
    }


    public void SetWith(DeviceSourceToRawValue rawValue) {
        m_deviceSourceRawValue = rawValue;

        m_onDisplayName.Invoke(rawValue.m_displayName);
        m_onProductName.Invoke(rawValue.m_productName); ;
        m_onManifacturer.Invoke(rawValue.m_manufacturer);
        m_onInterfaceName.Invoke(rawValue.m_interfacename);
        m_onDevicePath.Invoke(rawValue.m_devicePath);
        m_onDeviceButtonCount.Invoke(rawValue.m_booleanValue.Count);
        m_onDeviceAxisCount.Invoke(rawValue.m_axisValue.Count);

       

    }
    private void Update()
    {
        for (int i = 0; i < m_booleanGUI.Length; i++)
        {
            if (i < m_deviceSourceRawValue.m_booleanValue.Count)
            {
                m_booleanGUI[i].SetAsDisplay(true);
                m_booleanGUI[i].Set(i, m_deviceSourceRawValue.m_booleanValue[i].m_givenIdName, m_deviceSourceRawValue.m_booleanValue[i].m_value);
                GUI_HIDPathUniqueIDMono uniqueId = m_booleanGUI[i].GetComponent<GUI_HIDPathUniqueIDMono>();
                if (uniqueId != null)
                {
                    string p = HIDButtonStatic.GetIDPathAndButtonName(
                         m_deviceSourceRawValue.m_devicePath,
                         m_deviceSourceRawValue.m_booleanValue[i].m_givenIdName
                         );
                    uniqueId.SetUniquePathId(p);
                }
            }
            else
            {
                m_booleanGUI[i].SetAsDisplay(false);
                m_booleanGUI[i].Clear();
            }
            
        }
        for (int i = 0; i < m_axisGUI.Length; i++)
        {
            if (i < m_deviceSourceRawValue.m_axisValue.Count)
            {
                m_axisGUI[i].SetAsDisplay(true);
                m_axisGUI[i].Set(i, m_deviceSourceRawValue.m_axisValue[i].m_givenIdName, m_deviceSourceRawValue.m_axisValue[i].m_value);
                GUI_HIDPathUniqueIDMono uniqueId = m_axisGUI[i].GetComponent<GUI_HIDPathUniqueIDMono>();
                if (uniqueId != null)
                {
                   string p= HIDButtonStatic.GetIDPathAndButtonName(
                        m_deviceSourceRawValue.m_devicePath,
                        m_deviceSourceRawValue.m_axisValue[i].m_givenIdName
                        );
                    uniqueId.SetUniquePathId(p);
                }
            }
            else
            {
                m_axisGUI[i].SetAsDisplay(false);
                m_axisGUI[i].Clear();
            }
           
        }
    }
}


public class DeviceToLogUtility
{

    public static  string CreateExportLog(DeviceSourceToRawValue rawValue, bool hasJsonCapabilities)
    {
        StringBuilder sb = new StringBuilder();

        string path = rawValue.m_devicePath;
        sb.AppendLine("# Basic Info");
        sb.AppendLine("Path: `" + rawValue.m_devicePath + "`  ");
        sb.AppendLine("Display Name: `" + rawValue.m_displayName + "`  ");
        sb.AppendLine("Product Name: `" + rawValue.m_productName + "`  ");
        sb.AppendLine("Manufacurer: `" + rawValue.m_manufacturer + "`  ");
        sb.AppendLine("Interface: `" + rawValue.m_interfacename + "`  ");
        sb.AppendLine("Buttons count: `" + rawValue.m_booleanValue.Count + "`  ");
        sb.AppendLine("Axis count: `" + rawValue.m_axisValue.Count + "`  ");

        sb.AppendLine("##  All Buttons");
        sb.AppendLine("``` ");
        sb.AppendLine(string.Join(" ", rawValue.m_booleanValue.Select(k => k.m_givenIdName)));
        sb.AppendLine("``` ");
        sb.AppendLine("##  All Axis");

        sb.AppendLine("``` ");
        sb.AppendLine(string.Join(" ", rawValue.m_axisValue.Select(k => k.m_givenIdName)));
        sb.AppendLine("``` ");
        sb.AppendLine("##  All Buttons Ref");

        sb.AppendLine("``` ");
        sb.AppendLine(string.Join("\n", rawValue.m_booleanValue.Select(k => HIDButtonStatic.GetIDPathAndButtonName(path, k.m_givenIdName)).ToArray()));
        sb.AppendLine("``` ");

        sb.AppendLine("##  All Axis ref ");
        sb.AppendLine("``` ");
        sb.AppendLine(string.Join("\n", rawValue.m_axisValue.Select(k => HIDButtonStatic.GetIDPathAndButtonName(path, k.m_givenIdName)).ToArray()));
        sb.AppendLine("``` ");

        if (hasJsonCapabilities) { 
            sb.AppendLine("# Capabilities");
            sb.AppendLine("``` json");
            sb.AppendLine(rawValue.m_capabilities + "  ");
            sb.AppendLine("``` ");
        }

        return sb.ToString();
    }


}