using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SleepyExperimentBooleanToSendMono : MonoBehaviour
{
    public UnityString m_onMessageSent;
    [System.Serializable]
    public class UnityString : UnityEvent<string> { }
    // Start is called before the first frame update
    //void Start()
    //{
    //    InvokeRepeating("PingOneSecond", 0, 1);
    //    InvokeRepeating("PingTenSecond", 0, 10);
    //}

    //public bool m_1second;
    //public bool m_10seconds;


    //public void PingOneSecond()
    //{
    //    m_1second = !m_1second;
    //    m_onMessageSent.Invoke("bool:PingOneSecond:" + m_1second);
    //}
    //public void PingTenSecond()
    //{
    //    m_10seconds = !m_10seconds;
    //    m_onMessageSent.Invoke("bool:PingTenSecond:" + m_10seconds);

    //}

    public void PushBoolean(string name, bool value)
    {

        m_onMessageSent.Invoke("bool:"+ name + ":" + value);

    }
    public void PushStringCommandLineFromText(string text)
    {

        foreach (var line in text.Split("\n"))
        {
            m_onMessageSent.Invoke(line);
        }  
    }
}
