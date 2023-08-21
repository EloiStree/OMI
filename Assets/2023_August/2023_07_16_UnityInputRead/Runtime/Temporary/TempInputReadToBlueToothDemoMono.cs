using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInputReadToBlueToothDemoMono : MonoBehaviour
{

    public ListOfAllDeviceAsIdBoolFloatMono m_source;

public string m_idMenuLeft ="/XboxOneGamepadAndroid|>select";
public string m_idMenuRight ="/XboxOneGamepadAndroid|>start";
public string m_idLeftShoulder ="/XboxOneGamepadAndroid|>leftShoulder";
public string m_idRightShoulder ="/XboxOneGamepadAndroid|>rightShoulder";
public string m_idButtonSouth ="/XboxOneGamepadAndroid|>buttonSouth";
public string m_idButtonWest = "/XboxOneGamepadAndroid|>buttonWest";
public string m_idButtonNorth = "/XboxOneGamepadAndroid|>buttonNorth";
public string m_idButtonEast = "/XboxOneGamepadAndroid|>buttonEast";
public string m_idButtonJoyLeft = "/XboxOneGamepadAndroid|>leftStickPress";
public string m_idButtonJoyRight = "/XboxOneGamepadAndroid|>rightStickPress";
public string m_idRightTrigger ="/XboxOneGamepadAndroid|>rightTrigger";
public string m_idLeftTrigger ="/XboxOneGamepadAndroid|>leftTrigger";

public string m_idRightStickX ="/XboxOneGamepadAndroid|>rightStick x";
public string m_idRightStickY ="/XboxOneGamepadAndroid|>rightStick y";
public string m_idPadX ="/XboxOneGamepadAndroid|>dpad x";
public string m_idPadY ="/XboxOneGamepadAndroid|>dpad y";
public string m_idLeftStickX ="/XboxOneGamepadAndroid|>leftStick x";
public string m_idLeftStickY ="/XboxOneGamepadAndroid|>leftStick y";

    public float m_axisPushInSeconds=0.1f;
    public float m_a;
    void Start()
    {
        InvokeRepeating("PushBLEJoystick", 0, m_axisPushInSeconds);
    }
    public float m_jlx;
    public float m_jly;
    public float m_jrx;
    public float m_jry;
    public float m_dpx;
    public float m_dpy;
    public float m_sideMarge = 0.3f;

    public X m_value;


    [System.Serializable]
    public class X { 
        public DefaultBooleanChangeListener m_joyLeftLeft;
        public DefaultBooleanChangeListener m_joyLeftRight;
        public DefaultBooleanChangeListener m_joyLeftUp;
        public DefaultBooleanChangeListener m_joyLeftDown;

        public DefaultBooleanChangeListener m_joyRighLeft;
        public DefaultBooleanChangeListener m_joyRighRight;
        public DefaultBooleanChangeListener m_joyRighUp;
        public DefaultBooleanChangeListener m_joyRighDown;


        public DefaultBooleanChangeListener m_dpadLeft;
        public DefaultBooleanChangeListener m_dpadRight;
        public DefaultBooleanChangeListener m_dpadUp;
        public DefaultBooleanChangeListener m_dpadDown;
    }



    public void PushBLEJoystick() {

        //Debug.Log("DD ");
        if (!this.isActiveAndEnabled) return;
        PushAxis(m_idLeftStickX, "",out m_jlx );
        PushAxis(m_idLeftStickY, "" ,out m_jly );
        PushAxis(m_idRightStickX, "", out m_jrx);
        PushAxis(m_idRightStickY, "", out m_jry);
        PushAxis(m_idPadX, "", out m_dpx);
        PushAxis(m_idPadY, "", out m_dpy);

        bool changed = false;
        m_value.m_joyLeftLeft.SetBoolean(m_jlx < -1f + m_sideMarge, out changed);

       // Debug.Log("HHHMm " + m_value.m_joyLeftLeft.GetBoolean() + "  " + (m_value.m_joyLeftLeft.GetBoolean() ? -1 : 0));
        if (changed) {
         //   Debug.Log("HHHMm CC " + m_value.m_joyLeftLeft.GetBoolean() + "  " +( m_value.m_joyLeftLeft.GetBoolean() ? -1 : 0));
            m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jlh%", m_value.m_joyLeftLeft.GetBoolean()? -1 : 0));
        }
        m_value.m_joyLeftRight.SetBoolean(m_jlx > 1f - m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jlh%", m_value.m_joyLeftRight.GetBoolean() ? 1 : 0));

        m_value.m_joyLeftDown.SetBoolean(m_jly < -1f + m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jlv%", m_value.m_joyLeftDown.GetBoolean() ? -1 : 0));
        m_value.m_joyLeftUp.SetBoolean(m_jly > 1f - m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jlv%", m_value.m_joyLeftUp.GetBoolean() ? 1 : 0));


        m_value.m_joyRighLeft.SetBoolean(m_jrx < -1f + m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jrh%", m_value.m_joyRighLeft.GetBoolean() ? -1 : 0));
        m_value.m_joyRighRight.SetBoolean(m_jrx > 1f - m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jrh%", m_value.m_joyRighRight.GetBoolean() ? 1 : 0));

        m_value.m_joyRighUp.SetBoolean(m_jry < -1f + m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jrv%", m_value.m_joyRighUp.GetBoolean() ? -1 : 0));
        m_value.m_joyRighDown.SetBoolean(m_jry > 1f - m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;", "jrv%", m_value.m_joyRighDown.GetBoolean() ? 1:0));


        m_value.m_dpadLeft.SetBoolean(m_dpx < -1f + m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(m_value.m_dpadLeft.GetBoolean() ? "al. ;" : "al' ;");
        m_value.m_dpadRight.SetBoolean(m_dpx > 1f - m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(m_value.m_dpadRight.GetBoolean() ? "ar. ;" : "ar' ;");

        m_value.m_dpadUp.SetBoolean(m_dpy > 1f - m_sideMarge  , out changed);
        if (changed) m_onXboxCommandAxis.Invoke(m_value.m_dpadUp.GetBoolean() ? "au. ;" : "au' ;");
        m_value.m_dpadDown.SetBoolean(m_dpy < -1f + m_sideMarge, out changed);
        if (changed) m_onXboxCommandAxis.Invoke(m_value.m_dpadDown.GetBoolean() ? "ad. ;" : "ad' ;");


        //bool found;
        //m_source.GetAxisFromPathNameId(m_idPadX, out found, out HIDAxisChangedReference dPadX);
        //m_source.GetAxisFromPathNameId(m_idPadY, out found, out HIDAxisChangedReference dPadY);
        ///THIS is why I created OMI boolean to avoid rewriting this shit all the time.

    }

    private void PushAxis(string axisId, string prefix, out float debugValue)
    {
        debugValue = 0;
        if (!this.isActiveAndEnabled) {
            return;
        }
        bool found = false;
        HIDAxisChangedReference axis = null;
        m_source.GetAxisFromPathNameId(axisId, out found, out axis);
        if (found) {
            debugValue = axis.m_axisThatChanged.m_value;
           // m_onXboxCommandAxis.Invoke(string.Format("{0}{1:0.00} ;\n",prefix , axis.m_axisThatChanged.m_value));
        }

    }

    public Eloi.PrimitiveUnityEvent_String m_onXboxCommandBoolean;
    public Eloi.PrimitiveUnityEvent_String m_onXboxCommandAxis;
    public void NotifyButtonChanged(HIDButtonChangedReference buttonChanged)
    {
        if (!this.isActiveAndEnabled) return;
        bool buttonValue = buttonChanged.m_booleanThatChanged.m_value;

       // Debug.Log("TestA " + buttonChanged.m_uniqueId + " " + buttonChanged.m_booleanThatChanged.m_value);
        if (CheckAndPushIfFound(buttonChanged,  m_idMenuLeft, "ml. ;", "ml' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idMenuRight, "mr. ;", "mr' ;")) { }
        //else if (CheckAndPushIfFound(buttonChanged, buttonValue, m_idMenuRight, "mc. ;", "mc' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idButtonSouth, "a. ;", "a' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idButtonWest, "x. ;", "x' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idButtonNorth, "y. ;", "y' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idButtonEast, "b. ;", "b' ;")) { }

        else if (CheckAndPushIfFound(buttonChanged,  m_idLeftTrigger, "lt. ;", "lt' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idLeftShoulder, "sbl. ;", "sbl' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idRightTrigger, "rt. ;", "rt' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idRightShoulder, "sbr. ;", "sbr' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idButtonJoyLeft, "jl. ;", "jl' ;")) { }
        else if (CheckAndPushIfFound(buttonChanged,  m_idButtonJoyRight, "jr. ;", "jr' ;")) { }



    }

    private bool CheckAndPushIfFound(HIDButtonChangedReference buttonChanged, string id, string press, string release)
    {
        bool isFound = Eloi.E_StringUtility.AreEquals( buttonChanged.m_uniqueId, id, true, true);
        //Debug.Log("TestC " + buttonChanged.m_uniqueId + " " + id +" #"+isFound);
        if (isFound) {

            bool buttonValue = buttonChanged.m_booleanThatChanged.m_value;
          //  Debug.Log("TestB " + buttonChanged.m_uniqueId + " " + buttonChanged.m_booleanThatChanged.m_value);
            m_onXboxCommandBoolean.Invoke(buttonValue ? press : release);
        }

        return isFound;
    }

}
