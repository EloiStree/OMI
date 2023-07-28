using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SerialPortConnectionLog : MonoBehaviour
{
    public string m_debugConnectionLog;
    public string m_debugDisconnectionLog;
    public string m_splitter =", ";
    public Eloi.PrimitiveUnityEvent_String m_onConnectList;
    public Eloi.PrimitiveUnityEvent_String m_onDisconnectList;

    public List<string> m_connectionList= new List<string>();
    public List<string> m_disconnectionList = new List<string>();
    public int m_maxElementInList = 10;


    public void PushNewConnection(string comConnection)
    {
        m_connectionList.Add(comConnection);
        while(m_connectionList.Count>=m_maxElementInList)
            m_connectionList.RemoveAt(0);
        m_debugConnectionLog = string.Join(m_splitter, m_connectionList);
        m_onConnectList.Invoke(m_debugConnectionLog);
    }
    public void PushNewConnection(string [] comConnection)
    {
        foreach (var item in comConnection)
            PushNewConnection(item);

    }
    public void PushLostConnection(string comConnection)
    {
        m_disconnectionList.Add(comConnection);
        while (m_disconnectionList.Count >= m_maxElementInList)
            m_disconnectionList.RemoveAt(0);
        m_debugDisconnectionLog = string.Join(m_splitter, m_disconnectionList);
        m_onDisconnectList.Invoke(m_debugDisconnectionLog);
    }
    public void PushLostConnection(string[] comConnection)
    {
        foreach (var item in comConnection)
            PushLostConnection(item);

    }
}
