using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MonitorIdInformationGroup : MonoBehaviour
{
    public UI_MonitorIdInformation[] m_uiMonitors;


    public void SetUiMonitor(int index, string windowIdName, string monitorName,
        string constructorId, string registerId)
    {
        if (index < m_uiMonitors.Length)
        {
            m_uiMonitors[index].m_onDeviceWindowIndexName.Invoke(windowIdName);
            m_uiMonitors[index].m_onDevicePortChannelName.Invoke(monitorName);
            m_uiMonitors[index].m_onDeviceConstructorId.Invoke(constructorId);
            m_uiMonitors[index].m_onDeviceWinRegisterId.Invoke(registerId);
        }
    }

    public void GetPanelCount(out int panelCount) { panelCount = m_uiMonitors.Length; }
    public void Display(int index)
    {
        if (index < m_uiMonitors.Length)
            m_uiMonitors[index].gameObject.SetActive(true); 
    }
    public void Hide(int index) {
        if(index < m_uiMonitors.Length)
        m_uiMonitors[index].gameObject.SetActive(false);
    }

}
