using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MessageReceivedEvent : UnityEvent<TimedMessage> { }
[System.Serializable]
public class MessageReceivedAsStringEvent : UnityEvent<string> { }

[System.Serializable]
public struct TimedMessage
{
    public long m_whenReceived;
    public string m_message;
    public TimedMessage(string message)
    {
        m_message = message;
        m_whenReceived = (long)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }
}
