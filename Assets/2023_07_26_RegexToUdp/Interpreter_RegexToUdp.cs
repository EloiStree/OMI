using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter_RegexToUdp : AbstractInterpreterMono
{
    public RegexToUdpInterpreterRegisterMono m_regextoUdpRegister;
    public UDPThreadSender m_udpSender;

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        m_regextoUdpRegister.GetInterpreterFromText(command.GetLine(), out bool found, out RegexToUdpInterpreterContainer tmp);
        return found;
    }

    public override string GetName()
    {
        return "Regex to Udp Intepreter";
    }


    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {
        string cmd = command.GetLine();
        m_regextoUdpRegister.GetInterpreterFromText(cmd, out bool found, out RegexToUdpInterpreterContainer tmp);
        if (found) {
            bool useUTF8 = tmp.m_exportType == RegexToUdpInterpreterContainer.ExportType.UTF8;
            m_udpSender.SendMessageTo(tmp.m_ipv4AndPortToTarget  , cmd , useUTF8);
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