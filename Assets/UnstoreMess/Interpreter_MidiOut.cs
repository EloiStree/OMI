
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter_MidiOut : AbstractInterpreterMono
{

    public WindowMidiOutWrapperMono m_midiWindowOut;

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {

        return StartWith(ref command, "midi:", true) || StartWith(ref command, "midiraw:", true);
    }

    public override string GetName()
    {
        return "Midi Interpretor";
    }





    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {
        string cmd = command.GetLine().Trim().ToLower();

        long abosluteTime = 0;
        long channel = 1;
        long noteNumber = 1;
        long duration = 500;
        long velocity = 100;

        if (cmd.IndexOf("midi:") == 0)
        {
            string toFilter = cmd.Substring("midi:".Length);
            string[] p = toFilter.Split(' ');
            for (int i = 0; i < p.Length; i++)
            {
                string token = p[i].Trim().ToLower();
                if (token == "release" || token == "r") {
                    m_midiWindowOut.ReleaseAll();
                }
                else if (token.Length > 0)
                {
                    if (token == "cr")
                        channel = UnityEngine.Random.Range(1, 16);
                    else if (token == "nr")
                        noteNumber = UnityEngine.Random.Range(0, 127);
                    else if (token == "tr")
                        abosluteTime = UnityEngine.Random.Range(0, 800);
                    else if (token == "dr")
                        duration = UnityEngine.Random.Range(100, 800);
                    else if (token == "vr")
                        velocity = UnityEngine.Random.Range(0, 127);

                    else if (CharIntegerToValue(token, 'c', out long valueC))
                        channel = valueC;
                    else if (CharIntegerToValue(token, 'n', out long valueN))
                        noteNumber = valueN;
                    else if (CharIntegerToValue(token, 'd', out long valueD))
                        duration = valueD;
                    else if (CharIntegerToValue(token, 'v', out long valueV))
                        velocity = valueV;
                    else if (CharIntegerToValue(token, 't', out long valueT))
                        abosluteTime = valueT;

                    try
                    {
                        if (m_midiWindowOut != null)
                            m_midiWindowOut.SendNoteRaw((int)channel, (int)noteNumber, (int)velocity, (int)duration, abosluteTime);
                    }
                    catch (Exception)
                    {

                    }
                }

            }
            
        }
        if (cmd.IndexOf("midiraw:") == 0)
        {

            //  m_midiWindowOut.se
        }


        succedToExecute.SetAsFinished(true);
    }

    private static bool CharIntegerToValue(string text, in char c, out long value)
    {
        value = 0;
        if (text.IndexOf("" + c) > -1)
        {
            text = text.Replace("" + c, "");
            if (long.TryParse(text, out value))
            {
                return true;
            }
        }
        return false;
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

