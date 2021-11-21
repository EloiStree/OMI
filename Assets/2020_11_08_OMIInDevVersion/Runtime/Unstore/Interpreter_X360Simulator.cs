using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter_X360Simulator : AbstractInterpreterMono
{
    public WindowXboxSimulator m_xboxSim;

    public bool m_autoStart = true;

    private void Awake()
    {
        if (m_autoStart) { 
        Invoke("CreateConnection",0.1f);
        }
    }

    private void CreateConnection()
    {
        m_xboxSim.CreateConnection();
    }

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        return StartWith(ref command, "x360:", true);
    }

    public override string GetName()
    {
        return "Xbox Simulator";
    }

    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {

        string cmd = command.GetLine().Trim();
        string cmdLow = command.GetLine().ToLower();
        string[] token = cmd.Split(':');

        if (token.Length >= 2)
        {
            string action = token[1].Trim().ToLower();
            //keyrecord:start
            if (token.Length == 3) {
                if (action == "startsim")
                {

                    int value = 0;
                    if (int.TryParse(token[2].Trim().ToLower(), out value))
                    {
                        m_xboxSim.CreateConnection(value);
                    }
                    else
                    {
                        m_xboxSim.CreateConnection();
                    }

                }
                else
                {
                    string valueUsed = token[2].ToLower().Trim();
                    bool isBoolBalue= valueUsed == "1" || valueUsed == "0" || valueUsed == "true" || valueUsed == "false" ;
                    bool boolValue= valueUsed == "1" || valueUsed == "true";
                    if (isBoolBalue)
                    {

                        bool enumFound;
                        XInputBoolable boolable;
                        m_boolAlias.Get(action, out enumFound, out boolable);
                        if (enumFound)
                        {
                            Simulate(boolable, boolValue);
                            Debug.Log("X360 Bool 1:" + boolable+" "+ boolValue);

                        }
                    }
                    else { 
                    

                        bool enumFound;
                        XInputFloatableValue boolable;
                        m_floatAlias.Get(action, out enumFound, out boolable);
                        if (enumFound)
                        {
                            float value = 0;
                            if (float.TryParse(token[2].Trim().ToLower(), out value))
                            {
                                Simulate(boolable,value);
                                Debug.Log("X360 Bool 2:" + boolable);
                            }

                        }
                    }
                }
               



            }
            else if (token.Length == 2)
            {
                if (action == "startsim")
                {
                    m_xboxSim.CreateConnection();
                }
                else if (action == "aboard" || action == "aboardall")
                {
                    m_xboxSim.AboardAll();
                }
                else
                {
                    bool enumFound;
                    XInputBoolable boolable;
                    m_boolAlias.Get(token[1], out enumFound, out boolable);
                    if (enumFound) {
                        Simulate(boolable);
                        Debug.Log("X360 Bool 3:"+ boolable);
                    }


                }

               
            }
            
        }


        succedToExecute.SetAsFinished(true);
    }

    private void Simulate(XInputFloatableValue boolable, float value)
    {
        value = Mathf.Clamp(value, -1f, 1f);
        switch (boolable)
        {
            case XInputFloatableValue.TriggerLeft:
                m_xboxSim.SetTriggerLeft(value);
                break;
            case XInputFloatableValue.TriggerRight:
                m_xboxSim.SetTriggerRight(value);
                break;
            case XInputFloatableValue.JoystickLeftEast:
                m_xboxSim.SetAxisLeftHorizonal(value);
                break;
            case XInputFloatableValue.JoystickLeftNorth:
                m_xboxSim.SetAxisLeftVertical(value);
                break;
            case XInputFloatableValue.JoystickLeftSouth:
                m_xboxSim.SetAxisLeftVertical(-value);
                break;
            case XInputFloatableValue.JoystickLeftWest:
                m_xboxSim.SetAxisLeftHorizonal(-value);
                break;
            case XInputFloatableValue.JoystickRightEast:
                m_xboxSim.SetAxisRightHorizonal(value);
                break;
            case XInputFloatableValue.JoystickRightNorth:
                m_xboxSim.SetAxisRightVertical(value);
                break;
            case XInputFloatableValue.JoystickRightSouth:
                m_xboxSim.SetAxisRightVertical(-value);
                break;
            case XInputFloatableValue.JoystickRightWest:
                m_xboxSim.SetAxisRightHorizonal(-value);
                break;
            default:
                break;
        }
    }

    private void Simulate(XInputBoolable boolable)
    {
        Simulate(boolable, true);
        Simulate(boolable, false);
    }
    private void Simulate(XInputBoolable boolable, bool valueOn)
    {
        switch (boolable)
        {
            case XInputBoolable.TriggerRight:
                m_xboxSim.SetTriggerRight(valueOn ? 1f : 0f);
                break;
            case XInputBoolable.TriggerLeft:
                m_xboxSim.SetTriggerLeft(valueOn ? 1f : 0f);
                break;
            case XInputBoolable.SideButtonRight:
                m_xboxSim.SetButtonSideRight(valueOn);
                break;
            case XInputBoolable.SideButtonLeft:
                m_xboxSim.SetButtonSideLeft(valueOn);
                break;
            case XInputBoolable.ArrowLeft:
                m_xboxSim.SetArrowLeft(valueOn);
                break;
            case XInputBoolable.ArrowRight:
                m_xboxSim.SetArrowRight(valueOn);
                break;
            case XInputBoolable.ArrowUp:
                m_xboxSim.SetArrowUp(valueOn);
                break;
            case XInputBoolable.ArrowDown:
                m_xboxSim.SetArrowDown(valueOn);
                break;
            case XInputBoolable.ButtonA:
                m_xboxSim.SetButtonDown_A(valueOn);
                break;
            case XInputBoolable.ButtonB:
                m_xboxSim.SetButtonRight_B(valueOn);
                break;
            case XInputBoolable.ButtonY:
                m_xboxSim.SetButtonUp_Y(valueOn);
                break;
            case XInputBoolable.ButtonX:
                m_xboxSim.SetButtonLeft_X(valueOn);
                break;
            case XInputBoolable.MenuBackButton:
                m_xboxSim.SetButtonBack(valueOn);
                break;
            case XInputBoolable.StartButton:
                m_xboxSim.SetButtonStart(valueOn);
                break;
            case XInputBoolable.JoystickLeft:
                m_xboxSim.SetButtonJoystickLeft(valueOn);
                break;
            case XInputBoolable.JoystickRight:
                m_xboxSim.SetButtonJoystickRight(valueOn);
                break;
            default:
                break;
        }
    }

    public override void WhatIsYourRequirementFor(ref ICommandLine command, out ICommandExecutioninformation executionInfo)
    {
        executionInfo = new CommandExecutionInformation(false, false, false, false);
    }

    public override string WhatWillYouDoWith(ref ICommandLine command)
    {
        return "";

    }
    public GroupOfAlias<XInputBoolable> m_boolAlias = new GroupOfAlias<XInputBoolable>(
     new EnumAlias<XInputBoolable>(XInputBoolable.TriggerLeft, "tl"),
     new EnumAlias<XInputBoolable>(XInputBoolable.TriggerRight, "tr"),
     new EnumAlias<XInputBoolable>(XInputBoolable.SideButtonLeft, "sbl", "bl"),
     new EnumAlias<XInputBoolable>(XInputBoolable.SideButtonRight, "sbr", "br"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ArrowLeft, "al"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ArrowRight, "ar"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ArrowDown, "ad"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ArrowUp, "au"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ButtonA, "a", "ba", "paddown", "pd"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ButtonB, "b", "bb", "padright", "pr"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ButtonX, "x", "bx", "padleft", "pl"),
     new EnumAlias<XInputBoolable>(XInputBoolable.ButtonY, "y", "by", "padup", "pu"),
     new EnumAlias<XInputBoolable>(XInputBoolable.MenuBackButton, "m", "menu", "b", "back"),
     new EnumAlias<XInputBoolable>(XInputBoolable.StartButton, "s", "start"),
     new EnumAlias<XInputBoolable>(XInputBoolable.JoystickLeft, "jl"),
     new EnumAlias<XInputBoolable>(XInputBoolable.JoystickRight, "jr"));

    public GroupOfAlias<XInputFloatableValue> m_floatAlias = new GroupOfAlias<XInputFloatableValue>(
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.TriggerLeft, "tl"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.TriggerRight, "tr"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickLeftEast, "jlr", "jle"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickLeftNorth, "jlu", "jln"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickLeftSouth, "jld", "jls"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickLeftWest, "jll", "jlw"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickRightEast, "jrr", "jre"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickRightNorth, "jru", "jrn"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickRightSouth, "jrd", "jrs"),
     new EnumAlias<XInputFloatableValue>(XInputFloatableValue.JoystickRightWest, "jrl", "jrw"));
}
