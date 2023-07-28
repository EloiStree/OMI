using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using UnityEngine;

public class SerialPortOpenCloseNotificationMono : MonoBehaviour
{

    public SerialPortOpenCloseNotification m_trackedPort;
    public Eloi.PrimitiveUnityEvent_StringArray m_onCurrentPortAvailable;
    public Eloi.PrimitiveUnityEvent_StringArray m_onNewPortFound;
    public Eloi.PrimitiveUnityEvent_StringArray m_onLostPortFound;


    public bool m_refreshOnAwake;

    private void Awake()
    {
        if (m_refreshOnAwake) {
            RefreshComListInfoNoNotification();
            RefreshComListInfoNoNotification();
        }
    }

    [ContextMenu("Refresh List of COM port no notification")]
    public void RefreshComListInfoNoNotification()
    {
        m_trackedPort.RefreshComListInfo();
       
    }



    [ContextMenu("Refresh List of COM port")]
    public void RefreshComListInfo()
    {
        m_trackedPort.RefreshComListInfo();
        if (m_trackedPort.m_newPort.Length > 0)
        {
            m_onNewPortFound.Invoke(m_trackedPort.m_newPort.ToArray());
        }
        if (m_trackedPort.m_lostPort.Length > 0) { 
            m_onLostPortFound.Invoke(m_trackedPort.m_lostPort.ToArray());
        }
        if (m_trackedPort.m_newPort.Length > 0 || m_trackedPort.m_newPort.Length > 0) {

            m_onCurrentPortAvailable.Invoke(m_trackedPort.m_currentPort.ToArray());
        }

    }
   

}


[System.Serializable]
public class SerialPortOpenCloseNotification
{

    public string[] m_currentPort;
    public string[] m_previousPort;
    public string[] m_newPort;
    public string[] m_lostPort;

    public void RefreshComListInfo()
    {
        m_previousPort = m_currentPort.ToArray();
        m_currentPort = SerialPort.GetPortNames().Distinct().ToArray();
        m_newPort = (m_currentPort.ToArray().Except(m_previousPort)).ToArray();
        m_lostPort = (m_previousPort.ToArray().Except(m_currentPort)).ToArray();
    }
}


