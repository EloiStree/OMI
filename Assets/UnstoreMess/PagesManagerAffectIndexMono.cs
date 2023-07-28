using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesManagerAffectIndexMono : MonoBehaviour
{
    public PagesManagerMono m_toAffect;
    public Eloi.PrimitiveUnityEvent_Int m_onIndexChanged;

    public int m_index;
    public bool m_loopingAtLimit;

    [ContextMenu("Go Next")]
    public void GoNext()
    {
        m_index++;
        if (m_index >= m_toAffect.m_pages.Count)
        {
            if (m_loopingAtLimit)
            {
                m_index = 0;
            }
            else
            {
                m_index = m_toAffect.m_pages.Count - 1;
            }
        }
        m_toAffect.DisplayOnlyAtIndex(m_index);
        m_onIndexChanged.Invoke(m_index);

    }
    [ContextMenu("Go previous")]
    public void GoPrevious()
    {
        m_index--;
        if (m_index < 0) {
            if (m_loopingAtLimit)
            {
                m_index = m_toAffect.m_pages.Count - 1;
            }
            else
            {
                m_index =0;
            }
        }
        m_toAffect.DisplayOnlyAtIndex(m_index);
        m_onIndexChanged.Invoke(m_index);

    }
    [ContextMenu("Go Start")]
    public void GoToStart()
    {
        m_index=0;
        m_toAffect.DisplayOnlyAtIndex(m_index);
        m_onIndexChanged.Invoke(m_index);

    }
    [ContextMenu("Go End")]
    public void GoToEnd()
    {
        m_index = m_toAffect.m_pages.Count - 1;
        m_toAffect.DisplayOnlyAtIndex(m_index);
        m_onIndexChanged.Invoke(m_index);
    }
}
