using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUI_HIDPathUniqueIDMono : MonoBehaviour
{
    public string m_pathUniqueID;
    public StringEvent m_onPathUniqueIdPush;


    public void SetUniquePathId(string uniqueID) {
        m_pathUniqueID = uniqueID;
    }

    [ContextMenu("Push ID")]
    public void PushID() {
        m_onPathUniqueIdPush.Invoke(m_pathUniqueID);
    }

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
}
