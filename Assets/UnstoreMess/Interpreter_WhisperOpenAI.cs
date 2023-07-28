using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interpreter_WhisperOpenAI : AbstractInterpreterMono
{



    public override string GetName()
    {
        return "Whisper";
    }

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        return command.GetLine().ToLower().IndexOf("whisper:") == 0;
    }


    private string GetValueOfCommand(string command)
    {
        return command.ToLower().Replace("whisper:", "").Trim();
    }

    public UnityEvent m_onWhisperDefaultStart;
    public UnityEvent m_onWhisperDefaultStop;
    public Eloi.PrimitiveUnityEvent_String m_onWhisperDefaultStartChannel;
    public Eloi.PrimitiveUnityEvent_String m_onWhisperDefaultStopChannel;

    public override void TranslateToActionsWithStatus(
        ref ICommandLine command,
        ref ExecutionStatus succedToExecute)
    {

        

        bool succed = false;
        string cmd = command.GetLine();
        string cmdLow = cmd.ToLower().Trim();
        string[] tokens = cmdLow.Split(":");
        if (tokens.Length <= 2) {
            if (cmdLow == "whisper:start") { m_onWhisperDefaultStart.Invoke();
                succedToExecute.SetAsFinished(succed); return; }
            if (cmdLow == "whisper:stop") { m_onWhisperDefaultStop.Invoke();
                succedToExecute.SetAsFinished(succed); return; }
        
        }


        if (tokens.Length == 3) {
            if (tokens[1].Trim() == "start")
            {
                string name = tokens[2].Trim();
                m_onWhisperDefaultStartChannel.Invoke(name);
            }
            if (tokens[1].Trim() == "stop")
            {
                string name = tokens[2].Trim();
                m_onWhisperDefaultStopChannel.Invoke(name);
            }

        }
        succedToExecute.SetAsFinished(succed);
    }


    public override void WhatIsYourRequirementFor(ref ICommandLine command, out ICommandExecutioninformation executionInfo)
    {
        executionInfo = null;
    }

    public override string WhatWillYouDoWith(ref ICommandLine command)
    {
        return "Nothing the color format is not recognize";
    }

}