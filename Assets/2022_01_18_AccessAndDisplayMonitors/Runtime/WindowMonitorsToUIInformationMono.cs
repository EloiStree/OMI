using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMonitorsToUIInformationMono : MonoBehaviour
{
    public WindowMonitorsMono m_monitorInformation;
    public UI_MonitorIdInformationGroup m_uiMonitorDisplay;

    
    void Update()
    {

        Refresh();
        
    }

    private void Refresh()
    {
        int valideMonitorIndex = 0;
        var monitors = m_monitorInformation.m_nativeInfoGiven.m_nativeInformation;
        for (int i = 0; i < monitors.Length; i++)
        {
            if (monitors[i].m_devMode.dmPelsWidth > 0 &&
                monitors[i].m_devMode.dmPelsHeight > 0) {
                m_uiMonitorDisplay.SetUiMonitor(valideMonitorIndex,
                    monitors[i].m_display.DeviceName,
                    monitors[i].m_display.DeviceString,
                    monitors[i].m_display.DeviceID,
                    monitors[i].m_display.DeviceKey
                    );

                valideMonitorIndex++;
            }
        }
        m_uiMonitorDisplay.GetPanelCount(out int panelCount);
        for (int i = valideMonitorIndex; i <panelCount; i++)
        {
            m_uiMonitorDisplay.Hide(i);
        }
    }
}
