using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Interpreter_ExecutableFile : AbstractInterpreterMono
{
    public ExecutableFileRegister m_executableInTheProject;
    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        return IsValide(command);
    }

    private static bool IsValide(ICommandLine command)
    {
        return command.GetLine().ToLower().Trim().IndexOf("exefile:") == 0;
    }

    public override string GetName()
    {
        return "Executable Interpreter";
    }

    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {
        if (!IsValide(command)) {
            succedToExecute.StopWithError("Command is not valide");
                return ;
        }
        string cmd = command.GetLine().Trim().Substring(8);
        string [] tokens =cmd.Split(':');
        if (tokens.Length == 0)
        {
            succedToExecute.StopWithError("Command is not valide");
            return;
        }
      

        List<string> args = tokens.ToList();
        args.RemoveAt(0);
        ExecutableFile file;
        if (m_executableInTheProject.GetExecutableByNameWithExtension(tokens[0], out file))
        {
            ExecutableLauncher.LaunchExecutable(file.GetAbsolutePath(), string.Join(" ", args));
        }
        else if (m_executableInTheProject.GetExecutableByName(tokens[0], out file))
        {
            ExecutableLauncher.LaunchExecutable(file.GetAbsolutePath(), string.Join(" ", args));
        }
        else succedToExecute.StopWithError("Did not find the file:" + cmd);

    }

    public override void WhatIsYourRequirementFor(ref ICommandLine command, out ICommandExecutioninformation executionInfo)
    {
        executionInfo = new CommandExecutionInformation(false, false, false, false);
    }

    public override string WhatWillYouDoWith(ref ICommandLine command)
    {
        return "Try to launch an executable file store near the project.";
    }

    
}
