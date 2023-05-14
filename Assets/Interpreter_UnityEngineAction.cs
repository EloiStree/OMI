
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interpreter_UnityEngineAction : AbstractInterpreterMono
{
    


    public override string GetName()
    {
        return "Basic Unity Action";
    }

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        return command.GetLine().ToLower().IndexOf("unity:") == 0;
    }


    private string GetValueOfCommand(string command)
    {
        return command.ToLower().Replace("unity:", "").Trim();
    }

    public UnityEvent m_requestApplicationRestart;
    public override void TranslateToActionsWithStatus(
        ref ICommandLine command, 
        ref ExecutionStatus succedToExecute)
    {
        bool succed = false ;
        string cmd = command.GetLine();
        if (cmd.ToLower().StartsWith("unity:")) {
            cmd = cmd.Substring("unity:".Length);

            if (cmd.ToLower().StartsWith("open:") )
            {
                Application.OpenURL(cmd.Substring("open:".Length));
                succed = true;
            }
            if ( cmd.ToLower().StartsWith("openurl:"))
            {
                Application.OpenURL(cmd.Substring("openurl:".Length));
                succed = true;
            }
            if (cmd.ToLower().StartsWith("restart"))
            {

                m_requestApplicationRestart.Invoke();
                succed = true;
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
