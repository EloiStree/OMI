using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_NamedBooleanWithIndexMono : MonoBehaviour
{

    public int m_index;
    public string m_label;
    public bool m_isTrue;
    public bool m_isDisplay;
    

    public Eloi.PrimitiveUnityEvent_Int m_onIndex;
    public Eloi.PrimitiveUnityEvent_String m_onLabel;
    public Eloi.PrimitiveUnityEventExtra_Bool m_onIsTrue;
    public Eloi.PrimitiveUnityEvent_Bool m_onDisplay;
    // Start is called before the first frame update

    public void Set(int index, string label, bool isTrue) {
        m_index = index;
        m_label = label;
        m_isTrue = isTrue;
        RefreshUI();
    }

    private void RefreshUI()
    {
        m_onIndex.Invoke(m_index);
        m_onLabel.Invoke(m_label);
        m_onIsTrue.Invoke(m_isTrue);
        m_onDisplay.Invoke(m_isDisplay);
    }
    public void SetAsDisplay(bool setAsDisplay)
    {
        m_isDisplay = setAsDisplay;

    }

    public void Clear()
    {
        Set(0, "", false);
    }
}
