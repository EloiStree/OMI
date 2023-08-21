using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAndroidSensorThatAreHID : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
            Input.location.Start();
        #endif
    }

}
