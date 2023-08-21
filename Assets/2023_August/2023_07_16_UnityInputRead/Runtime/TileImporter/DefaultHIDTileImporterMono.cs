using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultHIDTileImporterMono : MonoBehaviour
{
    public HIDRefButtonDirectUniqueIdEvent m_onButtonObserver;
    public HIDRefAxisDirectUniqueIdEvent m_onAxisObserver;
    public void PushTextToBasicPathUniqueID(string text) {
        HIDTileImporter.ConvertTextToBasicPathUniqueID(text,
            out List<HIDRef_DeviceButtonDirectUniqueID> buttonToObserver,
            out List<HIDRef_DeviceAxisDirectUniqueID> axisToObserve);
        foreach (var item in buttonToObserver)
        {
            m_onButtonObserver.Invoke(item);
        }
        foreach (var item in axisToObserve)
        {
            m_onAxisObserver.Invoke(item);
        }


    }
}
public class HIDTileImporter { 
    public static void ConvertTextToBasicPathUniqueID(string text,
        out List<HIDRef_DeviceButtonDirectUniqueID> buttonToObserver,
        out List<HIDRef_DeviceAxisDirectUniqueID> axisToObserver) {

        buttonToObserver = new List<HIDRef_DeviceButtonDirectUniqueID>();
        axisToObserver = new List<HIDRef_DeviceAxisDirectUniqueID>();

        FileTileUtility.GetTile(text, out List<TileLine> lines , true);
        /*
        /XboxOneGamepadAndroid|>leftStickPress♦a_joystickLeft♦ ☗Inverse
        /XboxOneGamepadAndroid|>rightShoulder♦a_sbr
        /XboxOneGamepadAndroid|>leftShoulder♦a_sbl
        /XboxOneGamepadAndroid|>rightStickPress♦a_joystickright
        /XboxOneGamepadAndroid|>rightStick x♦a_joyRightOnLeft♦ -1 ♦ -0.2
        /XboxOneGamepadAndroid|>rightStick x♦a_joyRightOnRight♦ 0.2 ♦ 1
        /XboxOneGamepadAndroid|>rightStick x♦a_joyRightNotUse♦-0.1♦0.1♦ ☗Inverse
        /XboxOneGamepadAndroid|>rightStick y♦a_joyLRightOnDown♦ -1 ♦ -0.2
        /XboxOneGamepadAndroid|>rightStick y♦a_joyRightOnUp♦ 0.2 ♦ 1
        /XboxOneGamepadAndroid|>leftStick x♦a_joyLeftOnDown♦ -1 ♦ -0.2
        /XboxOneGamepadAndroid|>leftStick y♦a_joyLeftOnUp♦ 0.2 ♦ 1
        */

        foreach (var line in lines)
        {
            int count = line.GetCount();
            bool isInverse = false ;
            string idPath = count >= 1 ? line.GetValue(0) : "";
            string boolean = count >= 2 ? line.GetValue(1) : "";

            if (count == 2)
            {
                HIDRef_DeviceButtonDirectUniqueID o = new HIDRef_DeviceButtonDirectUniqueID();
                o.m_uniqueID = idPath;
                o.m_booleanName = boolean;
                buttonToObserver.Add(o);
                continue;
            }
            if (count == 3)
            {
                List<ShogiParameter> p = ShogiParameter.FindParametersInString(line.GetValue(2));
                if (ShogiParameter.HasParam(p, "☗Inverse", out ShogiParameter found))
                    isInverse = true;

                HIDRef_DeviceButtonDirectUniqueID o = new HIDRef_DeviceButtonDirectUniqueID();
                o.m_uniqueID = idPath;
                o.m_booleanName = boolean;
                o.m_buttonObserved.m_valueIsTrue = isInverse ? false : true;
                buttonToObserver.Add(o);
                continue;
            }
            if (count == 4)
            {
               
                HIDRef_DeviceAxisDirectUniqueID o = new HIDRef_DeviceAxisDirectUniqueID();
                o.m_uniqueID = idPath;
                o.m_booleanName = boolean;
                if (float.TryParse(line.GetValue(2), out float v1) && float.TryParse(line.GetValue(3), out float v2)) {
                    o.m_axisObserved.m_betweenMin = v1;
                    o.m_axisObserved.m_betweenMax = v2;
                    axisToObserver.Add(o);
                    continue;
                }
            }
            if (count == 5)
            {
                List<ShogiParameter> p = ShogiParameter.FindParametersInString(line.GetValue(4));
                if (ShogiParameter.HasParam(p, "☗Inverse", out ShogiParameter found))
                    isInverse = true;

                HIDRef_DeviceAxisDirectUniqueID o = new HIDRef_DeviceAxisDirectUniqueID();
                o.m_uniqueID = idPath;
                o.m_booleanName = boolean;
                o.m_axisObserved.m_inverse = isInverse;
                if (float.TryParse(line.GetValue(2), out float v1) && float.TryParse(line.GetValue(3), out float v2))
                {
                    o.m_axisObserved.m_betweenMin = v1;
                    o.m_axisObserved.m_betweenMax = v2;
                    axisToObserver.Add(o);
                    continue;
                }
            }

        }

    
    }



}
