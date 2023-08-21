using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class ListOfAllDeviceAsIdBoolFloatMono : MonoBehaviour
{
    public List<string> m_devicesName;
    public List<DeviceSourceToRawValue> m_devicesId;


    public NamedBooleanDeviceChanged m_onBooleanChanged;

    void Start()
    {
        Refresh();

        InputSystem.onEvent += OnReceivedHIDReport;
    }

    public void CheckIfExistsAndRemove(InputDevice[] devices)
    {
        foreach (var device in devices)
        {
            RemoveFromPath(device.path);
        }
    }

  
    public void CheckIfExistsAndRemoveFromPath(string[]  device)
    {
        foreach (var path in device)
        {
            RemoveFromPath(path);
        }
    }
    private void RemoveFromPath(string path)
    {
        m_devicesId = m_devicesId.Where(k => !(k.m_devicePath == path)).ToList();
    }


    private void OnDestroy()
    {
        InputSystem.onEvent -= OnReceivedHIDReport;
    }
    public bool m_refreshStateOnHIDReport;
    private  void OnReceivedHIDReport(InputEventPtr eventPtr, InputDevice device)
    {
        if(m_refreshStateOnHIDReport)
            RequestToRefresh(device);

     
    }

    public void GetAllPaths(out string[] devicePaths)
    {
        devicePaths= m_devicesId.Select(k => k.m_devicePath).ToArray();
    }

    private void RequestToRefresh(InputDevice device)
    {
        if (ignoreGamepad && device is Gamepad gamepad) return;
        else if (ignoreJoystick && device is Joystick joystick) return;
        else if (ignoreKeyboard && device is Keyboard keyboard) return;
        else if (ignoreMouse && device is Mouse mouse) return;
        if (IsBan(device.path)) return;

        if (device == null)
            return;
        GetDeviceRegistered(device, out bool found, out DeviceSourceToRawValue deviceInfo);
        if (!found)
        {
            deviceInfo = CreateDeviceWith(device, true);
        }
        if (deviceInfo == null)
            return;
        if (device == null)
            return;
        {

            ReadOnlyArray<InputControl> controls = device.allControls;
            ExtractValueForDevice(deviceInfo, device, controls);
        }
    }

    public Dictionary<string, HIDButtonChangedReference> m_buttonRef = new Dictionary<string, HIDButtonChangedReference>();

    public void GetDeviceInfoFromPath(string devicePath, out bool found, out DeviceSourceToRawValue device)
    {
        Eloi.E_CodeTag.DirtyCode.Info("Won't work if use threads. Need dico but I am a bit sleepy.");
        foreach (var item in m_devicesId)
        {
            if (item.m_devicePath == devicePath)
            {
                found = true;
                device = item;
                return;
            }
        }
        found = false;
        device = null;
    }

    public Dictionary<string, HIDAxisChangedReference> m_axisRef = new Dictionary<string, HIDAxisChangedReference>();


    public void GetButtonFromPathNameId(string path, string axisName, out bool found, out HIDButtonChangedReference buttonRef)
    {
        GetButtonFromPathNameId(HIDButtonStatic.GetIDPathAndButtonName(path, axisName), out found, out buttonRef);
    }
    public void GetButtonFromPathNameId(string pathNamedId, out bool found, out HIDButtonChangedReference buttonRef )
    {
        if (m_buttonRef.ContainsKey(pathNamedId))
        {
            found = true;
            buttonRef = m_buttonRef[pathNamedId];
        }
        else {
            found = false;
            buttonRef = null;
        }
    }

    public int m_totalButton;
    public int m_totalAxis;
    public void GetButtonTotalCount(out int buttonCount) { buttonCount = m_buttonRef.Count; }
    public void GetAxisTotalCount(out int axisCount) { axisCount = m_axisRef.Count; }

    public void GetAxisFromPathNameId(string path, string axisName, out bool found, out HIDAxisChangedReference buttonRef)
    {
        GetAxisFromPathNameId(HIDButtonStatic.GetIDPathAndButtonName(path, axisName), out found, out buttonRef);
    }
        public void GetAxisFromPathNameId(string pathNamedId, out bool found, out HIDAxisChangedReference buttonRef)
    {
        if (m_axisRef.ContainsKey(pathNamedId))
        {
            found = true;
            buttonRef = m_axisRef[pathNamedId];
        }
        else
        {
            found = false;
            buttonRef = null;
        }

    }

    [ContextMenu("Refresh Unique Id Pointers")]
    public void RefreshUniqueIdPointers()
    {
        m_buttonRef.Clear();
        m_axisRef.Clear();
        foreach (var item in m_devicesId)
        {
            foreach (var c in item.m_booleanValue)
            {
                string id = HIDButtonStatic.GetID(in item, in c);
                m_buttonRef.Add(id, new HIDButtonChangedReference(item, c));
            }
            foreach (var c in item.m_axisValue)
            {
                string id = HIDButtonStatic.GetID(in item, in c);
                m_axisRef.Add(id, new HIDAxisChangedReference(item, c));
            }
        }
        GetButtonTotalCount(out m_totalButton);
        GetAxisTotalCount(out m_totalAxis);
    }

    private bool IsBan(string path)
    {
        path = path.Trim();
        for (int i = 0; i < m_deviceBanList.Count; i++)
        {
            if (m_deviceBanList[i].Trim() == path)
                return true;
        }
        return false;
    }

    public List<InputDeviceDescription> m_unsupportedDevice;
    public List<string> m_unsupportedDeviceName;
    public bool ignoreJoystick;
    public bool ignoreKeyboard;
    public bool ignoreMouse;
    public bool ignoreGamepad;

    public List<string> m_deviceBanList = new List<string>();


    [ContextMenu("Clear Refresh")]
    public void ClearRefresh() {

        m_devicesId.Clear();
        Refresh();
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        m_unsupportedDevice = InputSystem.GetUnsupportedDevices();
        m_unsupportedDeviceName = m_unsupportedDevice.Where(k => k.product.Trim().Length > 0).Select(k => k.product.Trim()).Distinct().ToList();
        m_devicesName = InputSystem.devices.Where(k => k.path.Trim().Length>0).Select(k => k.path.Trim()).ToList();

        InputDevice[] devices = InputSystem.devices.Where(k => k != null  && k.path.Trim().Length >  0 && !IsBan(k.path)).ToArray();

        foreach (InputDevice device in devices)
        {
            if(device!=null)
                RequestToRefresh(device);
        }
    }

    public void NotifyNewDeviceDetected() {

        ClearRefresh();
        RefreshUniqueIdPointers();
    }


    public List<string> m_unsupportedType= new List<string>();
    private void Update()
    {
        Refresh();
    }

    private void ExtractValueForDevice(DeviceSourceToRawValue deviceInfo, InputDevice device,  ReadOnlyArray<InputControl> controls)
    {
        // Iterate through each button control on the gamepad
        foreach (InputControl control in controls)
        {
            
             if (control is ButtonControl)
            {
                ButtonControl buttonControl = (ButtonControl)control;

                if (buttonControl.wasPressedThisFrame)
                {
                   // Debug.Log("Button " + buttonControl.displayName + " was pressed on " + device.displayName);
                }
                else if (buttonControl.wasReleasedThisFrame)
                {
                    //Debug.Log("Button " + buttonControl.displayName + " was released on " + device.displayName);
                }


                deviceInfo.PushOut (control.name, control.IsPressed());
            }
            else if (control is AxisControl)
            {
                AxisControl axisControl = (AxisControl)control;
                float axisValue = axisControl.ReadValue();

                deviceInfo.PushOut(control.name, axisValue);

            }
            else if (control is KeyControl)
            {
                KeyControl c = (KeyControl)control;
                deviceInfo.PushOut(control.name, control.IsPressed());


            }
            else if (control is Vector3Control)
            {
                Vector3Control c = (Vector3Control)control;
                deviceInfo.PushOut(c.name + " " + c.x.name, c.x.value);
                deviceInfo.PushOut(c.name + " " + c.y.name, c.y.value);
                deviceInfo.PushOut(c.name + " " + c.z.name, c.z.value);

            }
            else if (control is Vector2Control)
            {
                Vector2Control c = (Vector2Control)control;
                deviceInfo.PushOut(c.name + " " + c.x.name, c.x.value);
                deviceInfo.PushOut(c.name + " " + c.y.name, c.y.value);

            }
            else if (control is IntegerControl)
            {
                IntegerControl c = (IntegerControl)control;
                deviceInfo.PushOut(c.name + " " + c.name, c.value);

            }
              
            else if (control is TouchControl)
            {
                TouchControl c = (TouchControl)control;

            }
            //else if (control is PointerControl)
            //{
            //    ButtonControl c = (ButtonControl)control;

            //}
            else if (control is QuaternionControl)
            {
                QuaternionControl c = (QuaternionControl)control;

            }
            else if (control is StickControl)
            {
                StickControl c = (StickControl)control;
                deviceInfo.PushOut(c.name + " " + c.x.name, c.x.value);
                deviceInfo.PushOut(c.name + " " + c.y.name, c.y.value);

            }
           
            else
            {
               // deviceInfo.m_inputDebug += "\nU " + control.name + " " + control.ReadDefaultValueAsObject();
            }
            
        }
    }

    private DeviceSourceToRawValue CreateDeviceWith(InputDevice device, bool addToRegister)
    {
        DeviceSourceToRawValue a =
         new DeviceSourceToRawValue()
         {
             m_unityId = device.deviceId,
             m_displayName = device.displayName,
             m_interfacename = device.description.interfaceName,
             m_capabilities = device.description.capabilities,
             m_deviceClass = device.description.deviceClass,
             m_manufacturer = device.description.manufacturer,
             m_devicePath = device.path,
             m_booleanValue = new List<DeviceSourceToRawValue.NamedBooleanValue>(),
             m_axisValue= new List<DeviceSourceToRawValue.NamedFloatValue>(),
             m_isSupported = !m_unsupportedDeviceName.Contains(device.description.product.Trim()),
             m_source = device,
             m_productName = device.description.product
        };

        if (addToRegister) {
            a.AddBooleanListener(m_onBooleanChanged);
            m_devicesId.Add(a);
        }
        return a;
    }

    private void GetDeviceRegistered(InputDevice device, out bool found, out DeviceSourceToRawValue deviceInfo)
    {
        if (device != null) { 
            for (int i = 0; i < m_devicesId.Count; i++)
            {
                if (m_devicesId[i].m_devicePath.Trim() == device.path.Trim())
                {
                    found = true;
                    deviceInfo = m_devicesId[i];
                    return;
                }
            }
        }
        found = false;
        deviceInfo = null;
    }

    public void GetFromIndex(int index, out bool found, out DeviceSourceToRawValue device)
    {
        if (index < 0 || index >= m_devicesId.Count)
        {
            found = false;
            device = null;
            return;
        }
        found = true;
        device = m_devicesId[index];
    }

    public void GetFromPath(string devicePath, out bool found, out int index, out DeviceSourceToRawValue device)
    {
        devicePath = devicePath.Trim();
        for (int i = 0; i < m_devicesId.Count; i++)
        {
            if (m_devicesId[i].m_devicePath == devicePath) {
                found = true;
                device = m_devicesId[i];
                index = i;
                return;
            }
        }
        found = false;
        device = null;
        index = -1;
    }
}

public delegate void NamedBooleanDeviceChanged(DeviceSourceToRawValue deviceInfo, DeviceSourceToRawValue.NamedBooleanValue booleanThatChanged, bool newValue);


[System.Serializable]
public class DeviceSourceToRawValue
{
    public string m_displayName;
    public int m_unityId;
    public string m_productName;
    public string m_manufacturer;
    public string m_interfacename;
    public string m_capabilities;
    public string m_deviceClass;
    public bool m_isSupported;

    public InputDevice m_source;
    public string m_devicePath;

    public List<NamedBooleanValue> m_booleanValue = new List<NamedBooleanValue>();
    public List<NamedFloatValue> m_axisValue = new List<NamedFloatValue>();

    public NamedBooleanDeviceChanged m_onBooleanChanged;

    public void AddBooleanListener(NamedBooleanDeviceChanged listener)
    {
        m_onBooleanChanged -= listener;
        m_onBooleanChanged += listener;

    }
    public void RemoveBooleanListener(NamedBooleanDeviceChanged listener)
    {
        m_onBooleanChanged -= listener;
    }

    public void AddListener()
    {
        InputSystem.onEvent -= OnButtonEvent;
        InputSystem.onEvent += OnButtonEvent;
    }
    public void RemoveListener()
    {
        InputSystem.onEvent -= OnButtonEvent;
    }
    private void OnButtonEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device != m_source)
            return;

        RefreshAllBooleanValue();
    }

    private void RefreshAllBooleanValue()
    {
        for (int i = 0; i < m_booleanValue.Count; i++)
        {
            bool previous= m_booleanValue[i].m_value;
            bool current = m_booleanValue[i].m_source.IsPressed();
            bool changed = previous!=current;
            m_booleanValue[i].m_value = current;
            if (changed) { 
            
               if(m_onBooleanChanged!=null)
                   m_onBooleanChanged.Invoke(this, m_booleanValue[i], current);           
            }
        }
    }

    public void Get(string nameid, out bool found, out NamedBooleanValue container)
    {
        if (nameid == null || nameid.Length <= 0)
        {
            found = false;
            container = null;
            return;
        }
        for (int i = 0; i < m_booleanValue.Count; i++)
        {
            if (nameid.Length == m_booleanValue[i].m_givenIdName.Length)
            {
                if (nameid[0] == m_booleanValue[i].m_givenIdName[0] &&
                    nameid == m_booleanValue[i].m_givenIdName)
                {
                    found = true;
                    container = m_booleanValue[i];
                    return;
                }
            }
        }
        found = false;
        container = null;

    }
    public void Get(string nameid, out bool found, out NamedFloatValue container)
    {

        if (nameid == null || nameid.Length <= 0)
        {
            found = false;
            container = null;
            return;
        }
        for (int i = 0; i < m_axisValue.Count; i++)
        {
            if (nameid.Length == m_axisValue[i].m_givenIdName.Length)
            {
                if (nameid[0] == m_axisValue[i].m_givenIdName[0] &&
                    nameid == m_axisValue[i].m_givenIdName)
                {
                    found = true;
                    container = m_axisValue[i];
                    return;
                }
            }
        }
        found = false;
        container = null;
    }

    [System.Serializable]
    public class InputControlNamedValue
    {
        public string m_givenIdName;
        public InputControl m_source;
    }

    [System.Serializable]
    public class NamedBooleanValue : InputControlNamedValue
    {
        public bool m_value;
        public string m_uniqueId;
        public string GetGUID()
        {
            if(m_uniqueId==null) m_uniqueId=Guid.NewGuid().ToString();
            return m_uniqueId;
        }
    }
    [System.Serializable]
    public class NamedFloatValue : InputControlNamedValue
    {
        public float m_value;
    }

    public void PushOut(string name, bool isTrue)
    {
      
        bool changed = false;
        Get(name, out bool found, out NamedBooleanValue container);
        if (!found)
        {
            NamedBooleanValue nb = new NamedBooleanValue()
            {
                m_givenIdName = name,
                m_source = m_source,
                m_value = isTrue
            };
            changed = true;
            m_booleanValue.Add(nb);
        }
        else
        {
            changed = container.m_value != isTrue;
            container.m_value = isTrue;
        }
        if (changed)
        {
            if (m_onBooleanChanged != null)
                m_onBooleanChanged.Invoke(this, container, isTrue);
        }

    }
    public void PushOut(string name, float axisValue)
    {
        Get(name, out bool found, out NamedFloatValue container);
        if (!found)
        {
            NamedFloatValue nb = new NamedFloatValue()
            {
                m_givenIdName = name,
                m_source = m_source,
                m_value = axisValue
            };
            m_axisValue.Add(nb);
        }
        else
        {
            container.m_value = axisValue;
        }
    }
}
