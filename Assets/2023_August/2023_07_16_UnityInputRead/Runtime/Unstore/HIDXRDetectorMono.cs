using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HIDXRDetectorMono : MonoBehaviour
{
    public string[] m_pathOfVRApp = new string[] { "/HeadTrackingOpenXR" };
    public bool m_isInXRApp;
    public Eloi.PrimitiveUnityEventExtra_Bool m_onIsXRApp;
    public string[] test;


    private void Start()
    {
        Invoke("CheckForVR", 1);
    }
    public void CheckForVR()
    {
        InputSystem.FlushDisconnectedDevices();
        test = InputSystem.devices.Select(k=>k.path).ToArray();
        m_isInXRApp = InputSystem.devices.Where(k => IsOneOfTrackedPath(k.path)).ToArray().Length>0;
        m_onIsXRApp.Invoke(m_isInXRApp);
    }

    private bool IsOneOfTrackedPath(string path)
    {
        foreach (var item in m_pathOfVRApp)
        {
            if (path == item)
                return true;
        }
         return false;
    }

}
