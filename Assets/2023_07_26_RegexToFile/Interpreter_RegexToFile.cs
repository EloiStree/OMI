using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Interpreter_RegexToFile : AbstractInterpreterMono
{
    public RegexToFileInterpretorRegisterMono m_regexRegister;
    //public UDPThreadSender m_udpSender;

    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        m_regexRegister.m_doubleInterpreter.GetInterpreterFromText(command.GetLine(),
            out bool found,
            out RegexToDoubleDateFileInterpreterContainer tmp1);

        if (!found) { 
            m_regexRegister.m_directoryInterpreter.GetInterpreterFromText(command.GetLine(),
                        out  found,
                        out RegexToCommandAsNewFileInterpreterContainer tmp2);

        }

        return found;
    }

    public override string GetName()
    {
        return "Regex to file Intepreter";
    }

    public uint m_index;
    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {
        Eloi.E_CodeTag.DirtyCode.Info("Write file take time and it should be delegate on a side thread to avoid waiting");
        string cmd = command.GetLine();

        m_regexRegister.m_doubleInterpreter.GetInterpreterFromText(cmd,
           out bool found,
           out RegexToDoubleDateFileInterpreterContainer tmp1);

        Debug.Log(string.Join("--", cmd, found, JsonUtility.ToJson(tmp1)));
        Eloi.E_GeneralUtility.GetTimeULongIdWithNow(out ulong idDate);
        string id = idDate + "" + m_index + ".txt";
        m_index++;

        if (found) {

            Eloi.E_FilePathUnityUtility.GetDirectoryPathOf(tmp1.m_targetFilePath,
                out string pathDirectory);
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }
            if (!File.Exists(tmp1.m_targetFilePath)) {
                File.WriteAllText(tmp1.m_targetFilePath,"");
            }
            if (tmp1.m_exportType == StringToExportFileType.Override) { 
                File.WriteAllText(tmp1.m_targetFilePath, cmd);
            }

            if (tmp1.m_exportType == StringToExportFileType.AppendStart) {
                string t = File.ReadAllText(tmp1.m_targetFilePath);
                File.AppendAllText(tmp1.m_targetFilePath, cmd+ "\n" +t);
            }
            if (tmp1.m_exportType == StringToExportFileType.AppendEnd) { 
                File.AppendAllText(tmp1.m_targetFilePath, "\n"+cmd);
            }

            Debug.Log(string.Join("-d-", tmp1.m_targetFilePath, found, JsonUtility.ToJson(tmp1)));
         


            if (tmp1.m_changeHappenFilePath.Trim().Length > 0) { 
                Eloi.E_FilePathUnityUtility.GetDirectoryPathOf(tmp1.m_changeHappenFilePath,
                    out  pathDirectory);
                if (!Directory.Exists(pathDirectory))
                {
                    Directory.CreateDirectory(pathDirectory);
                }
                File.WriteAllText(tmp1.m_changeHappenFilePath, ""+idDate);
            }
            succedToExecute.SetAsFinished(true);
            return;

        }

        m_regexRegister.m_directoryInterpreter.GetInterpreterFromText(command.GetLine(),
                        out found,
                        out RegexToCommandAsNewFileInterpreterContainer tmp2);
        if (found)
        {
            if ( !Directory.Exists(tmp2.m_whereToCreateFileFolderPath) ) {
                Directory.CreateDirectory(tmp2.m_whereToCreateFileFolderPath);
            }
            string path = tmp2.m_whereToCreateFileFolderPath + "/" + id;
            File.WriteAllText(path, cmd);
            succedToExecute.SetAsFinished(true);
            Debug.Log(string.Join("-h-", path, cmd, JsonUtility.ToJson(tmp2)));
            return;
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