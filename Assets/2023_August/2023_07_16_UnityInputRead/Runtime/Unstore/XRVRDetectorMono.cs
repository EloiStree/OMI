using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XRVRDetectorMono : MonoBehaviour
{
    public string [] m_devicesName;
    public UnityEvent m_isNotXR;
    public UnityEvent m_isXR;


    void Start()
    {
        // Check if the XR device is present
        bool isVRDevicePresent = XRSettings.isDeviceActive;
        m_devicesName = XRSettings.supportedDevices.ToArray();
        if (isVRDevicePresent)
        {
                m_isXR.Invoke();
        }
        else { 
                m_isNotXR.Invoke();
        }

        bool isVRSupported = XRSettings.supportedDevices.Contains("VR");

        // Check if the XR device is an Oculus VR device
        bool isOculusVR = XRSettings.loadedDeviceName.ToLower().Contains("oculus");

        // Check if the XR device is a SteamVR device
        bool isSteamVR = XRSettings.loadedDeviceName.ToLower().Contains("openvr");

        // Print the results to the console
        Debug.Log("Is VR Device Present: " + isVRDevicePresent);
        Debug.Log("Is VR Supported: " + isVRSupported);
        Debug.Log("Is Oculus VR: " + isOculusVR);
        Debug.Log("Is SteamVR: " + isSteamVR);
    }
}