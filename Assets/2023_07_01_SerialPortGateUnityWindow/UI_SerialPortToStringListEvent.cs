using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SerialPortToStringListEvent : MonoBehaviour
{


    public SerialPortUnityGateMono m_serialPortLinked;
    public Eloi.PrimitiveUnityEvent_StringArray m_onCurrentPortDetected;
    public Eloi.PrimitiveUnityEvent_StringArray m_onNewPortDetected;
    public Eloi.PrimitiveUnityEvent_StringArray m_onLostPortDetected;

    public void RefreshUI()
    {
       // m_onCurrentPortDetected.Invoke(m_serialPortLinked.)
        
    }
}
