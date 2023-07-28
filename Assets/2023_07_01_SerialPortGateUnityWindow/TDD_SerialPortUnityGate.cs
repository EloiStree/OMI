using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_SerialPortUnityGate : MonoBehaviour
{
    public string m_comToOpen = "COM11";
    public int m_baudRate = 115200;
    public string m_msgA = "";
    public string m_msgB = "";
    void Start()
    {

        SerialPortUnityGateStatic.GetInstance().OpenSerialPort(m_comToOpen,m_baudRate);

    }

    [ContextMenu("SendMessageA")]
    public void SendMessageA()
    {
        SerialPortUnityGateStatic.GetInstance().SendTextTo(m_comToOpen, m_msgA);
    }
    [ContextMenu("SendMessageB")]
    public void SendMessageB()
    {
        SerialPortUnityGateStatic.GetInstance().SendTextTo(m_comToOpen, m_msgB);
    }


    private void OnApplicationQuit()
    {
        SerialPortUnityGateStatic.GetInstance().CloseAllOpen();
    }
    private void OnDestroy()
    {
        SerialPortUnityGateStatic.GetInstance().CloseAllOpen();
    }
}


