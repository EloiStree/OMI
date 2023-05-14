using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win32ThreadIdChangeObserver : MonoBehaviour
{
    public int m_currentPointer;
    public int m_previousPointer;

    public int m_parentProcessId;
    public int m_childrenId;
    public int m_previousId;
    public int m_previousChildrenId;
    public Eloi.PrimitiveUnityEvent_Int m_newProcessId;
    public Eloi.PrimitiveUnityEvent_Int m_newProcessIdActiveChildren;
    public Eloi.PrimitiveUnityEvent_Int m_previousProcessId;
    public Eloi.PrimitiveUnityEvent_Int m_previousProcessIdActiveChildren;

    public bool m_useTheUnityOne;
    public List<int> m_unityIds = new List<int>();
    
    private void Awake()
    {

        SetCurrentAsUnityIds();
    }

    private void SetCurrentAsUnityIds()
    {
        WindowIntPtrUtility.GetCurrentChildrenFocusProcessId(out IntPtrWrapGet id);
        WindowIntPtrUtility.GetParentAndChildOf(id, out IntPtrWrapGet parentProcessId, out IntPtrWrapGet childrenId);
        
        m_unityIds.Clear();
        m_unityIds.Add(parentProcessId.GetAsInt());
        IntPtrWrapGet[] pts = WindowIntPtrUtility.GetProcessIdChildrenWindows(parentProcessId);
        for (int i = 0; i < pts.Length; i++)
        {
            m_unityIds.Add(pts[i].GetAsInt());
        }
    }
    public bool m_ignoreZeroProcess;
    void Update()
    {

        WindowIntPtrUtility.GetCurrentChildrenFocusProcessId(out IntPtrWrapGet pid);
        m_currentPointer = pid.GetAsInt();
        bool isNewProcess = false;
        if (m_ignoreZeroProcess) {
            isNewProcess = pid.GetAsInt() != m_previousPointer && pid.GetAsInt() != 0;
        }
        else isNewProcess = pid.GetAsInt() != m_previousPointer;
        if (isNewProcess) {
            
            E_CodeTag.ToCodeLater.Info("It is a good idea but I need to fine a 100% it is unity process and not a maybe it is");
            //if (!m_useTheUnityOne && IsUnityThreadId(pid.GetAsInt()))
            //{
            //}
            //else
            {
                m_previousId = m_parentProcessId;
                m_previousChildrenId = m_childrenId;
                WindowIntPtrUtility.GetParentAndChildOf(pid, out IntPtrWrapGet parentProcessId, out IntPtrWrapGet childrenId);
                m_parentProcessId = parentProcessId.GetAsInt();
                m_childrenId = childrenId.GetAsInt();
                m_newProcessId.Invoke(m_parentProcessId);
                m_newProcessIdActiveChildren.Invoke(m_childrenId);
                m_previousProcessId.Invoke(m_previousId);
                m_previousProcessIdActiveChildren.Invoke(m_previousChildrenId);

                m_previousPointer = m_currentPointer;

            }
        }
        
    }

    private bool IsUnityThreadId(int pid)
    {
        for (int i = 0; i < m_unityIds.Count; i++)
        {
            if (pid == m_unityIds[i])
                return true;
        }
        return false;
    }
}
