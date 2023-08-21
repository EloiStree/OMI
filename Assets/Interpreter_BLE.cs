
using System.Collections.Generic;
using UnityEngine;

public class Interpreter_BLE : AbstractInterpreterMono
{
    public BLSendDevicesManagerMono m_bleSendManager;
    public BLDeviceListMono m_searchInPaired;
    public UniqueIdOrAliasToStringChanelRegisterMono m_aliasToGroup = new UniqueIdOrAliasToStringChanelRegisterMono();
    public string m_lastReceived;   

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {

        return StartWith(ref command, "ble|", true);
    }

    public override string GetName()
    {
        return "Bluetooth Intepreter";
    }

    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {
        //if (!StartWith(ref command, "ble|", true)) return;

        string cmd = command.GetLine();
        string[] tokens = cmd.Split("|");
        for (int i = 0; i < tokens.Length; i++)
        {
            tokens[i] = tokens[i].Trim();
        }

        m_lastReceived = cmd;
        if (tokens.Length == 3)
        {

            string name = tokens[1];
            string msg = tokens[2];
            if (tokens[2].IndexOf(":") > -1)
            {
                m_bleSendManager.SendMessageToDevice(name, msg);
            }
            else
            {
                m_aliasToGroup.Get(tokens[1], true, out bool groupfound, out UniqueIdToListOfObject<string> group);
                if (groupfound)
                {
                    foreach (var item in group.m_linkedObject)
                    {
                        Eloi.E_CodeTag.SleepyCode.Info("Should be recurcive with protection.");
                        m_aliasToGroup.Get(item, true, out bool itemFound, out UniqueIdToListOfObject<string> subSearch);
                        if (itemFound)
                        {
                            foreach (var itemsub in subSearch.m_linkedObject)
                            {
                                SendMessage(itemsub, msg);
                            }
                        }
                        else { 
                            SendMessage(item, msg);
                        }
                    }
                }
                else
                {
                    SendMessage(name, msg);
                }
            }
        }
        //Debug.Log(string.Join("@@@", tokens));
    }

    private void SendMessage(string name, string msg)
    {
        //Debug.Log($"TT {name} {msg}");
        m_searchInPaired.SearchDeviceByName(name, out List<BLDevicePairedBasicInfo> devices);

        foreach (var item in devices)
        {
            m_bleSendManager.SendMessageToDevice(item.m_macAddress, msg);
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