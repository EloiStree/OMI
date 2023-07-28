using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SendDemo : MonoBehaviour
{
//    public OpenSerialPortAtStart serial;
    public string sendOn = "1";
    public string sendOff = "0";
    public string toSend = "";
    public bool m_switchLoop;
    private bool m_switchState;
    public StringEvent m_onPushMessage;

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { 
    
    }

    IEnumerator Start()
    {
        while (true) {
            yield return new WaitForSeconds(1);
           
                m_switchState = !m_switchState; 
            if (toSend.Length > 0)
            {
                toSend = m_switchState ? sendOff : sendOn;
                m_onPushMessage.Invoke(toSend);
            }
            
        }

    }


}