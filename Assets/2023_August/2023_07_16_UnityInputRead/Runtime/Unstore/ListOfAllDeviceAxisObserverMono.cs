using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListOfAllDeviceAxisObserverMono : MonoBehaviour
{
    [TextArea(0,5)]
    public string m_developerNote="Update can skip some pro gamer change, use thread is possible.";
    public HIDObserveUserIntentLinkToCurrentDeviceMono m_observer;

    public ListOfAllDeviceAsIdBoolFloatMono m_sourceObservered;
    public bool m_useUpdateToCheckChange;
    public Dictionary<object, bool> m_booleanStateByObject = new Dictionary<object, bool>();
    //public DicoFloatChangeObserversMono m_floatObserved;


    public OnObserveAxisChangeState m_onAxisObserverStateChanged;
    public OnBooleaNamedChanged m_onBooleanNamedChanged;

    [System.Serializable]
    public class OnObserveAxisChangeState : UnityEvent<HIDRef_DeviceAxisUniqueID, bool>
    {

    }
    [System.Serializable]
    public class OnBooleaNamedChanged : UnityEvent<string, bool>
    {

    }


    public void Update()
    {
        if(m_useUpdateToCheckChange)
        ObserveAxisChange();
    }

    public void Test() { }
    public void ObserveAxisChange()
    {
        foreach (var item in m_observer.m_axisDico.Values)
        {
          
            foreach (var i in item)
            {
                
                m_sourceObservered.GetAxisFromPathNameId(
                    i.m_uniqueID
                    , out bool found
                    , out HIDAxisChangedReference axisInfo);
                if (found)
                {
                    float currentValue = axisInfo.m_axisThatChanged.m_value;
                    i.m_axisObserver.CheckMinMax();
                    bool isInRange = currentValue >= i.m_axisObserver.m_betweenMin && currentValue <= i.m_axisObserver.m_betweenMax;
                    if (i.m_axisObserver.m_inverse)
                        isInRange = !isInRange;

                    // Debug.Log(string.Join(" ","Tick:", i.m_uniqueID, isInRange));
                    NotifyAxisBooleanState(i, isInRange);
                }
                else { 
                   // Debug.Log(string.Join(" ", "Not Found:", i.m_uniqueID));
                }

            }

        }

    }

    private void NotifyAxisBooleanState(HIDRef_DeviceAxisUniqueID deviceAxisObserved, bool isTrue)
    {
        if ( ! m_booleanStateByObject.ContainsKey(deviceAxisObserved) )
            m_booleanStateByObject.Add(deviceAxisObserved, isTrue);
        else if (m_booleanStateByObject[deviceAxisObserved] != isTrue)
        {
            m_booleanStateByObject[deviceAxisObserved] = isTrue;
            m_onBooleanNamedChanged.Invoke(deviceAxisObserved.m_booleanName, isTrue);
        }
    }

    public void DebugLogBooleanName(string name, bool isTrue) {

        Debug.Log("B:" + name + ":" + isTrue);
    }
}

