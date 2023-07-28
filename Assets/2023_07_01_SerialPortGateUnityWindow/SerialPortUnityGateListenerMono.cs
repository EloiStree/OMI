using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialPortUnityGateListenerMono : MonoBehaviour
{

    public string [] m_listenCom  = new string[] { "COM55" };

    public Dictionary<string, SerialComListener> m_listeners = new Dictionary<string, SerialComListener>();

    public Eloi.PrimitiveUnityEvent_String m_onReceivedMessage;
    public Eloi.PrimitiveUnityEvent_DoubleString m_onComMessage;




    [System.Serializable]
    public class SerialComListener {

        public string m_serialId;
        public SerialPortReaderThreadListener m_listener;

        public SerialComListener(string serialId)
        {
            m_serialId = serialId;
            m_listener = new SerialPortReaderThreadListener(serialId);
            m_listener.StartOrResetThread();
        }
    }




    private void Start()
    {
        AddListenerFromComId(m_listenCom);
    }

    public void AddListenerFromComId( string[] listenCom )
    {
        foreach (var item in listenCom)
        {
            AddListenerFromComId(item);
        }
    }
    public void AddListenerFromComId(string listenCom)
    {
        if ( ! m_listeners.ContainsKey(listenCom))
            m_listeners.Add(listenCom, new SerialComListener(listenCom));
    }

    private void Update()
    {
        foreach (string k in m_listeners.Keys)
        {
            m_listeners[k].m_listener.KeepAlive();
            m_listeners[k].m_listener.RecovertAndFlushReceivedMessage(out string[] messages);
            foreach (var item in messages)
            {
                //Debug.Log("M:" + item);
                m_onReceivedMessage.Invoke(item);
                m_onComMessage.Invoke(m_listeners[k].m_serialId, item);
               
            }

        }
    }

  

    private void OnApplicationQuit()
    {
        foreach (string k in m_listeners.Keys)
        {
            m_listeners[k].m_listener.StopAndCloseThread();
        }
    }
    private void OnDestroy()
    {
        foreach (string k in m_listeners.Keys)
        {
            m_listeners[k].m_listener.StopAndCloseThread();
        }
    }


    
}




public class SerialPortReaderThreadListener {


    public string m_serialPortId;
    public Queue<string> m_receivedMessage = new Queue<string>();

    private SerialPort m_givenPortConnection;
    private Thread m_createdThread;
    private bool m_isRunning = false;

    public DateTime m_keepAliveTimer;
    public float m_keepAliveTimeout = 5;


    public SerialPortReaderThreadListener(string givenPort)
    {
        m_serialPortId = givenPort;
        KeepAlive();
        TryToFetchTheSerialPort();
    }

    private void TryToFetchTheSerialPort()
    {
        SerialPortUnityGateStatic.GetInstance().
            GetExistingPort(m_serialPortId, out bool found, out m_givenPortConnection);
    }

    public bool HasMessage() { return m_receivedMessage.Count > 0; }
    public void RecovertAndFlushReceivedMessage(out string[] message) {
        message = m_receivedMessage.ToArray();
        m_receivedMessage.Clear();
    }

    public void KeepAlive() {
        m_keepAliveTimer =DateTime.Now;
    }

    public void StartOrResetThread()
    {
            StopAndCloseThread();
            m_isRunning = true;
            m_createdThread = new Thread(ReadSerialData);
            m_createdThread.Start();
            m_mustBeKilled = false;
    }

    public void StopAndCloseThread()
    {
        if (m_createdThread != null) { 
            m_isRunning = false;
            m_createdThread.Abort();
            m_mustBeKilled = true;
        }
    }
    public bool m_mustBeKilled;

    public bool IsThreadAlive() { return m_isRunning; }


    private void ReadSerialData()
    {
        KeepAlive();
        do
        {
            Thread.Sleep(100);
            try
            {
                if (m_givenPortConnection != null && m_givenPortConnection.IsOpen)
                {
                    string data = m_givenPortConnection.ReadLine();
                    if (!string.IsNullOrEmpty(data))
                    {
                        m_receivedMessage.Enqueue(data);
                    }
                }
                else {
                    TryToFetchTheSerialPort();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("SerialPort Exception:" + ex);
                StopAndCloseThread();
            };
        }
        while (IsToKeepAlive() && ! m_mustBeKilled);

        m_isRunning = false;
        StopAndCloseThread();
    }

    private bool IsToKeepAlive()
    {
        return (DateTime.Now - m_keepAliveTimer).TotalSeconds < m_keepAliveTimeout;
    }
}