using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_RawInputDeviceDropdownMono : MonoBehaviour
{
    public ListOfAllDeviceAsIdBoolFloatMono m_source;
    public DeviceSourceRawEvent m_onDeviceSelected;

    public int m_index;
    public List<string> m_labels;
    public Dropdown[] m_dropdowns;

    [System.Serializable]
    public class DeviceSourceRawEvent : UnityEvent<DeviceSourceToRawValue> { }
    [System.Serializable]
    public class DeviceSourceRawPathEvent : UnityEvent<string> { }
    public void SelectFromDropDown(int index)
    {
        m_index = index;
        m_source.GetFromIndex(index,out bool found, out DeviceSourceToRawValue device);
        if (found)
            m_onDeviceSelected.Invoke(device);
        RefreshDropdowns();
    }
    public void SelectFromPathId(string devicePath)
    {
        m_source.GetFromPath(devicePath, out bool found,out int index, out DeviceSourceToRawValue device);
        m_index = index;
        if (found)
            m_onDeviceSelected.Invoke(device);
        RefreshDropdowns();
    }


    private void RefreshDropdowns()
    {
        m_source.GetAllPaths(out string [] devicePaths);
        m_labels = devicePaths.ToList();
        foreach (var item in m_dropdowns)
        {
            item.ClearOptions();
            item.AddOptions(m_labels);
            item.SetValueWithoutNotify( m_index);
        }
    }

    public void Decrease()
    {
        if (m_index > 1)
            m_index--;
        SelectFromDropDown(m_index);
    }
    public void Increase()
    {
        if (m_index < m_source.m_devicesId.Count-1)
            m_index++;
        SelectFromDropDown(m_index);
    }
}
