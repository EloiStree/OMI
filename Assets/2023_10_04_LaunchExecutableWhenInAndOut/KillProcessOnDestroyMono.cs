using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KillProcessOnDestroyMono : MonoBehaviour
{
    public List<Process> m_processToKill = new List<Process>();
    public int m_toKillCount;
    void OnDestroy()
    {

        Kill();
    }
    void OnApplicationQuit()
    {
        Kill();
    }

    public void AddProcessToKill(Process process) {
        m_processToKill.Add(process);
        m_toKillCount = m_processToKill.Count;
    }
    private void Kill()
    {
        if (m_processToKill.Count > 0) { 
            foreach (var item in m_processToKill)
            {
                try {
                    if (item != null) { 
                        item.Kill();
                        item.Dispose();
                    }
                }
                catch (Exception) { 
            
                }

            }
        }
    }
}
