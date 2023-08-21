using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class ListOfAllDeviceDigitanalogMono : MonoBehaviour
{
    public ListOfAllDeviceAsIdBoolFloatMono m_source;
    [TextArea(0,20)]
    public string m_debugText;
    public StringEvent m_onNewDebugText;

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    public void Update()
    {
        Refresh();
    }

    public bool ignoreMouseAndKeyboard;
    public void Refresh() {

        StringBuilder sb = new StringBuilder();
        foreach (DeviceSourceToRawValue item in m_source.m_devicesId)
        {
            if (ignoreMouseAndKeyboard) { 
                if (item.m_devicePath.ToLower().IndexOf("mouse") > -1)
                    continue;
                if (item.m_devicePath.ToLower().IndexOf("keyboard") > -1)
                    continue;
            }
            sb.Append(item.m_devicePath + "|");

            for (int i = 0; i < item.m_booleanValue.Count; i++)
            {
                sb.Append(" ");
                sb.Append(item.m_booleanValue[i].m_value ? '1' : '0');
            }
            sb.Append(" |");
            for (int i = 0; i < item.m_axisValue.Count; i++)
            {
                sb.Append(" ");
                sb.AppendFormat("{0:0.00}",item.m_axisValue[i].m_value);
            }
            sb.AppendLine();



            sb.Append(item.m_devicePath + "|");

            for (int i = 0; i < item.m_booleanValue.Count; i++)
            {
                sb.Append(" D");
                sb.Append(i);
                sb.Append(" ");
                sb.Append(item.m_booleanValue[i].m_givenIdName);
            }
            sb.Append(" |");
            for (int i = 0; i < item.m_axisValue.Count; i++)
            {
                sb.Append(" A");
                sb.Append(i);
                sb.Append(" ");
                sb.Append(item.m_axisValue[i].m_givenIdName);
            }
            sb.AppendLine();
        }
        m_debugText = sb.ToString();
        m_onNewDebugText.Invoke(m_debugText);
    }
}
