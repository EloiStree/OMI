using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SerialPortGateEnums;

public class Test_InOutSerialPortMono : MonoBehaviour
{

    public Dictionary<string, SerialPortLayer> m_openedPort = new Dictionary<string, SerialPortLayer>();


    public void SendTextToTarget(in string text, in string portName = "COM1",
        in int baudRate = 9600,
        in int dataBits = 8,
        in Parity parity = Parity.None,
        in StopBits stopBits = StopBits.One)
    {
        if (!m_openedPort.ContainsKey(portName)) {
            try
            {
                SerialPortLayer serialPort =SerialPortAbstractLayerStatic.CreateFromSpecificPlatform(portName, baudRate, parity, dataBits, stopBits);
                serialPort.Open();
                m_openedPort.Add(portName.ToString(), serialPort);
            }
            catch (Exception e) {
                Debug.Log("Fail to open serial port:"+e.StackTrace);
            
            }
        }

        try
        {
            try {
                m_openedPort[portName].WriteLine(text);
            }
            catch (Exception e)
            {
                Debug.Log("Fail sent text to serial port:" + e.StackTrace);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public void CloseAllOpen() {

        string[] keys = m_openedPort.Keys.ToArray();
        foreach (var item in keys)
        {
            SerialPortLayer p = m_openedPort[item];
            try
            {
                if (p != null) {
                    p.Close();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail to close port: " + ex.Message);
            }
            m_openedPort.Remove(item);
        }
    
    }

    private void OnDestroy()
    {
        CloseAllOpen();
    }
    private void OnApplicationQuit()
    {

        CloseAllOpen();
    }
   
}

[System.Serializable]
public class SerialPortTargetInfo {
    public uint m_portId = 3;
    public uint m_baudRate = 9600;
    public uint m_dataBite = 8;
    public Parity m_parity;
    public StopBits m_stopBits = StopBits.One;
    public Handshake m_handShake;
}
