using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationFocusLockMono : MonoBehaviour
{

    public string m_;
    public bool m_isAppLocked;
    public string m_processNameOrId;
    public string m_restartAppPath = "";
    public bool m_isProcessExist;
    public IntPtrWrapGet m_process;
    public void LockTheApp()
    {
        m_isAppLocked = true;
    }
    public void UnLockTheApp()
    {
        m_isAppLocked = false;
    }

    public void LockTheApp(bool isAppMustBeLock)
    {
        m_isAppLocked = isAppMustBeLock;
    }

    private void Awake()
    {
        WindowIntPtrUtility.AllowProcessCurrentFocus();
    }


    private void Update()
    {
        if (m_isAppLocked)
        {
            WindowIntPtrUtility.GetProcessDisplayChildrenFromIdOrName(m_processNameOrId, out m_isProcessExist, out m_process);
            //windowimon
            // if (!m_isProcessExist) {
            if (m_process != null)
            {

                WindowIntPtrUtility.SetForegroundWindow(m_process);
            }
            //}
            //}
        }

    }


    [ContextMenu("Launch App")]
    public void LaunchTheApp() {
        System.Diagnostics.Process.Start(m_restartAppPath);
    }



}
