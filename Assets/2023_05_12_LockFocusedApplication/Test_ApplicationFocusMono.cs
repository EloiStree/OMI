using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Test_ApplicationFocusMono : MonoBehaviour
{
    public ApplicationFocusLockMono m_focusLockApp;
    [TextArea(0,10)]
    public string m_note;

    public string m_processNameTest;
    public string m_callbackDebug;
    public float m_focusTime = 60;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_focusLockApp.LockTheApp(true);
        yield return new WaitForSeconds(m_focusTime);
        m_focusLockApp.LockTheApp(false);

    }
    // Update is called once per frame
    void Update()
    {
        m_callbackDebug = JsonUtility.ToJson( Process.GetProcessesByName(m_processNameTest),true);
       ProcessesAccessMono access= ProcessesAccessInScene.Instance;
    }
}
