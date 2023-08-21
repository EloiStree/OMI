using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GenerateOfFullLogExportMono : MonoBehaviour
{

    public ListOfAllDeviceAsIdBoolFloatMono m_devicesSource;

    public Eloi.PrimitiveUnityEvent_String m_onLogEmitted;


    [ContextMenu("Produce Full Log")]
    public void ProduceFullLog()
    {
        if (m_devicesSource == null)
        {
            Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref m_devicesSource);
        }
        if (m_devicesSource == null)
        {
            return;
        }

        List<string> group = new List<string>();
        foreach (var item in m_devicesSource.m_devicesId)
        {
            group.Add(DeviceToLogUtility.CreateExportLog(item,false));
        }

        m_onLogEmitted.Invoke( string.Join("----------------------------------", group) );   
    }


}
