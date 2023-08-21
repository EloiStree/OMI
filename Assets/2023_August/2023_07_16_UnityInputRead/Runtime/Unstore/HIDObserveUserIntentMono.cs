using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HIDObserveUserIntentMono : MonoBehaviour
{
    public List<HIDSearch_OneDeviceButtonFromPath>      m_searchPathDeviceButton = new List<HIDSearch_OneDeviceButtonFromPath>();
    public List<HIDSearch_OneDeviceAxisFromPath>        m_searchPathDeviceAxis = new List<HIDSearch_OneDeviceAxisFromPath>();
    public List<HIDSearch_AnyDeviceButton>              m_searchAnyDeviceButton = new List<HIDSearch_AnyDeviceButton>();
    public List<HIDSearch_AnyDeviceAxis>                m_searchAnyDeviceAxis = new List<HIDSearch_AnyDeviceAxis>();
    public List<HIDSearch_ObserveAllButtonsFromIndex>   m_searchOneDeviceAllButtons = new List<HIDSearch_ObserveAllButtonsFromIndex>();

    public List<HIDSearch_ObserveAllButtonsFromPath> m_allButtonsFromPath = new List<HIDSearch_ObserveAllButtonsFromPath>();

    public List<HIDRef_DeviceButtonUniqueID> m_buttonFromUniqueId = new List<HIDRef_DeviceButtonUniqueID>();
    public List<HIDRef_DeviceAxisUniqueID> m_axisFromUniqueId = new List<HIDRef_DeviceAxisUniqueID>();
    public List<HIDRef_DeviceButtonDirectUniqueID> m_buttonDirectUniqueId = new List<HIDRef_DeviceButtonDirectUniqueID>();
    public List<HIDRef_DeviceAxisDirectUniqueID>   m_axisDirectUniqueId = new List<HIDRef_DeviceAxisDirectUniqueID>();


    public void Add(HIDRef_DeviceButtonDirectUniqueID value)
    {
        m_buttonDirectUniqueId.Add(value);
    }
    public void Add(HIDRef_DeviceAxisDirectUniqueID value)
    {
        m_axisDirectUniqueId.Add(value);
    }

}


[System.Serializable]
public class Intent_BooleanObserved
{

    public bool m_valueIsTrue = true;

}

[System.Serializable]
public class Intent_AxisObserved
{
    public float m_betweenMin;
    public float m_betweenMax;
    public bool m_inverse = false;

    public void CheckMinMax()
    {
        if (m_betweenMax < m_betweenMin)
        {
            float d = m_betweenMin;
            m_betweenMin = m_betweenMax;
            m_betweenMax = d;
        }
    }
}


[System.Serializable]
public class HIDRefButtonDirectUniqueIdEvent : UnityEvent<HIDRef_DeviceButtonDirectUniqueID> { }
[System.Serializable]
public class HIDRefAxisDirectUniqueIdEvent : UnityEvent<HIDRef_DeviceAxisDirectUniqueID> { }

[System.Serializable]
public class HIDRef_DeviceButtonDirectUniqueID
{
    public string m_uniqueID="";
    public string m_booleanName="";
    public Intent_BooleanObserved m_buttonObserved= new Intent_BooleanObserved();
}

[System.Serializable]
public class HIDRef_DeviceAxisDirectUniqueID
{
    public string m_uniqueID="";
    public string m_booleanName="";
    public Intent_AxisObserved m_axisObserved= new Intent_AxisObserved();
}



[System.Serializable]
public class HIDRef_DeviceButtonUniqueID
{
    public string m_uniqueID = "";
    public string m_booleanName = "";
    public Intent_ObserverHIDButton m_buttonObserver = new Intent_ObserverHIDButton();
}

[System.Serializable]
public class HIDRef_DeviceAxisUniqueID
{
    public string m_uniqueID = "";
    public string m_booleanName = "";
    public Intent_ObserverHIDAxis m_axisObserver = new Intent_ObserverHIDAxis();
}


[System.Serializable]
public class HIDRef_DeviceAxisUniqueIDWithRef
{
    public string m_uniqueID = "";
    public string m_booleanName = "";
    public Intent_ObserverHIDAxis m_axisObserver = new Intent_ObserverHIDAxis();
    public HIDAxisChangedReference m_sourceRef = null;
}



[System.Serializable]
public class HIDSearch_OneDeviceButtonFromPath
{
    public string m_deviceToExactPathObserver = "";
    public Intent_ObserverHIDButton m_buttonObserver = new Intent_ObserverHIDButton();
    public string m_booleanName = "";
}

[System.Serializable]
public class HIDSearch_OneDeviceAxisFromPath
{
    public string m_deviceToExactPathObserver = "";
    public Intent_ObserverHIDAxis m_axisObserver = new Intent_ObserverHIDAxis();
    public string m_booleanName = "";
}
[System.Serializable]
public class HIDSearch_OneDeviceButton
{
    public HIDSearchPattern_LookAroundWithIndex m_deviceToObserver = new HIDSearchPattern_LookAroundWithIndex();
    public Intent_ObserverHIDButton m_buttonObserver = new Intent_ObserverHIDButton();
    public string m_booleanName = "";
}

[System.Serializable]
public class HIDSearch_OneDeviceAxis
{
    public HIDSearchPattern_LookAroundWithIndex m_deviceToObserver = new HIDSearchPattern_LookAroundWithIndex();
    public Intent_ObserverHIDAxis m_axisObserver = new Intent_ObserverHIDAxis();
    public string m_booleanName = "";
}

[System.Serializable]
public class HIDSearch_AnyDeviceButton
{
    public HIDSearchPattern_LookAroundAny m_deviceToObserver = new HIDSearchPattern_LookAroundAny();
    public Intent_ObserverHIDButton m_buttonObserver = new Intent_ObserverHIDButton();
    public string m_booleanName = "";
}

[System.Serializable]
public class HIDSearch_AnyDeviceAxis
{
    public HIDSearchPattern_LookAroundAny m_deviceToObserver = new HIDSearchPattern_LookAroundAny();
    public Intent_ObserverHIDAxis m_axisObserver = new Intent_ObserverHIDAxis();
    public string m_booleanName = "";
}

[System.Serializable]
public class HIDSearch_ObserveAllButtonsFromIndex
{
    public HIDSearchPattern_LookAroundWithIndex m_deviceToObserver;
    public HIDObserverAllButtons m_buttonsObserver;
}
[System.Serializable]
public class HIDSearch_ObserveAllButtonsFromPath
{
    public string m_devicePath = "";
    public HIDObserverAllButtons m_buttonsObserver;
}
[System.Serializable]
public class HIDObserverAllButtons {
    public string m_frontOfButtons = "";
    public string m_replaceSpaceWith = "_";
}

[System.Serializable]
public class HIDSearchPattern_LookAroundWithIndex
{
    public string m_deviceToSearchRegex = "";
    public bool m_isExactly;
    public bool m_useAsRegex;
    public bool m_containsText;
    public ushort m_indexInFoundSearch = 0;
    public bool m_searchInPath = true;
    public bool m_searchInDisplayName = true;
    public bool m_searchInProductName = true;
    public bool m_searchInCapacity = true;
    public bool m_searchInManufacturerPlusDisplayName = true;
}
[System.Serializable]
public class HIDSearchPattern_LookAroundAny
{
    public string m_deviceToSearchRegex = "";
    public bool m_useAsRegex;
    public bool m_containsText;
    public bool m_searchInPath = true;
    public bool m_searchInDisplayName = true;
    public bool m_searchInProductName = true;
    public bool m_searchInCapacity = true;
    public bool m_searchInManufacturerPlusDisplayName = true;
}


[System.Serializable]
public class Intent_ObserverHIDButton
{
    public string m_buttonName = "";
    public bool m_trueIfPressed = true;
}

[System.Serializable]
public class Intent_ObserverHIDAxis
{

    public string m_axisName = "";
    public bool m_inverse = true;
    public float m_betweenMin;
    public float m_betweenMax;

    public void CheckMinMax() {
        if (m_betweenMax < m_betweenMin) {
            float d = m_betweenMin;
            m_betweenMin = m_betweenMax;
            m_betweenMax = d;
        }
    }
}