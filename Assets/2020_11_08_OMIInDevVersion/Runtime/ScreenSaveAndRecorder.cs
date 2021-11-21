using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaveAndRecorder : MonoBehaviour
{

    public MouseInformationAbstract m_mouseInfo;
    public Dictionary<string, ScreenPositionInPourcentBean> m_screenRecorder = new Dictionary<string, ScreenPositionInPourcentBean>();
    public UI_ServerDropdownJavaOMI m_target;


    public void Save(string name)
    {
        float lr, bt;
        m_mouseInfo.GetPourcent(out lr, out bt);
        if (!m_screenRecorder.ContainsKey(name))
            m_screenRecorder.Add(name, new ScreenPositionInPourcentBean(lr, bt));
        m_screenRecorder[name].SetLeftToRightValue(lr);
        m_screenRecorder[name].SetBotToTopValue(bt);
    }

    public void Recover(string name)
    {
        if (m_screenRecorder.ContainsKey(name)) {
            ScreenPositionInPourcentBean sp = m_screenRecorder[name];
            foreach (var item in m_target.GetJavaOMISelected())
            {
                item.MouseMoveInPourcent(
                    sp.GetLeftToRightValue(),
                    1f-sp.GetBotToTopValue());
            }
        }
    }
}
