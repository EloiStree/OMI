using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter_SerialPortWrite : AbstractInterpreterMono
{
    public UniqueIdToAliasRegisterMono m_aliasRegister;
    public GroupIdToListOfAliasAndComIdRegisterMono m_groupTarget;

    

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        return StartWith(ref command, "serial ", true) ;
    }

    public override string GetName()
    {
        return "Serial Port writer";
    }

    private void Awake()
    {
       // m_udpSender.ResetTheTargetFromAlias();
    }

    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {

        string cmd = command.GetLine().Trim().Substring("serial ".Length);
        //Debug.Log("CCCC" + cmd);
        int index = cmd.IndexOf("|");
        Eloi.E_StringUtility.SplitInTwo(in cmd, index, out string target, out string text);
        target = target.Trim();
        text= text.Trim();
        if (index > -1 && index + 1 < cmd.Length)
        {
            m_groupTarget.m_groupOfIdToListObject.Get(target, m_groupTarget.m_ignoreCase, out bool found, out UniqueIdToListOfObject<string> group);
            if (found)
            { 
                foreach (var item in group.m_linkedObject)
                {
                    TryToPushMessage(item, text);
                    //  Debug.Log("AAAA" + cmd);
                }

            }
            else {
                TryToPushMessage(target, text);
                // Debug.Log("FFFF" + cmd);
                //serial:COM52: y. 100 > y' ;
            }
        }
        succedToExecute.SetAsFinished(true);
    }

    private void TryToPushMessage(string target, string text)
    {
        if (SerialPortUnityGateStatic.GetInstance().IsRegistered(target))
        {
            SerialPortUnityGateStatic.GetInstance().SentMessagetoSerialPort(target, text);
            // Debug.Log("ss1|" + target+ "|"+ text);
        }
        else
        {
            m_aliasRegister.m_groupOfAlias.GetAny(target, true, out bool found, out UniqueIdToListOfAlias link, out bool isCom);
            if (found && link != null)
            {
                if (SerialPortUnityGateStatic.GetInstance().IsRegistered(link.m_uniqueId))
                    SerialPortUnityGateStatic.GetInstance().SentMessagetoSerialPort(link.m_uniqueId, text);
                // Debug.Log("ss2|" + target + "|" + text);
            }
            else {
                // Debug.Log("ss3|" + target + "|" + text);
            }
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
}