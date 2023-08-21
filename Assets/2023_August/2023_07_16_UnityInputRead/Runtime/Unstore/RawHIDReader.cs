//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.LowLevel;
//using System;

//public class RawHIDReader : MonoBehaviour
//{
//    private InputDevice inputDevice;
//    private int reportId = 0; // The report ID of the desired HID report

//    private void Start()
//    {
//        // Find the unsupported HID device
//        inputDevice = InputSystem.GetDeviceByName("Unsupported HID Device");

//        if (inputDevice == null)
//        {
//            Debug.LogError("Unsupported HID device not found");
//            return;
//        }

//        // Subscribe to the OnReceivedHIDReport event
//        InputSystem.onEvent += OnReceivedHIDReport;
//    }

//    private void OnDestroy()
//    {
//        // Unsubscribe from the OnReceivedHIDReport event
//        InputSystem.onEvent -= OnReceivedHIDReport;
//    }

//    private unsafe void OnReceivedHIDReport(InputEventPtr eventPtr, InputDevice device)
//    {
//        // Check if the event is for the desired HID device
//        if (device != inputDevice)
//            return;

//        // Check if the event is a HID report event
//        if (eventPtr.IsA<HIDReportEvent>())
//        {
//            // Cast the event to a HID report event
//            HIDReportEvent reportEvent = *(HIDReportEvent*)eventPtr.data;

//            // Check if the report ID matches
//            if (reportEvent.reportId == reportId)
//            {
//                // Retrieve the raw HID data
//                byte[] rawHIDData = new byte[reportEvent.sizeInBytes];
//                fixed (byte* dataPtr = rawHIDData)
//                {
//                    UnsafeUtility.MemCpy(dataPtr, reportEvent.data, reportEvent.sizeInBytes);
//                }

//                // Process the raw HID data as needed
//                Debug.Log("Raw HID Data: " + BitConverter.ToString(rawHIDData));
//            }
//        }
//    }
//}