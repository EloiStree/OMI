
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileImport_RegexFileInterpretor : MonoBehaviour
{
    public RegexToFileInterpretorRegisterMono m_register;

    public void ClearRegister()
    {
        m_register.Clear();
    }


    public void Load(params string[] filePath)
    {
        for (int i = 0; i < filePath.Length; i++)
        {
            if (File.Exists(filePath[i]))
            {
                ImportFileText(File.ReadAllText(filePath[i]));
            }
        }
    }

    private void ImportFileText(string text)
    {
        string[] lines = text.Split('\n');
        TileLine tokens;
        for (int i = 0; i < lines.Length; i++)
        {
            char[] l = lines[i].Trim().ToCharArray();
            if (l.Length > 0 && (l[0] == '#' || (l[0] == '/' && l[1] == '/')))
                continue;

            tokens = new TileLine(lines[i]);


            //Screenshot:♦SingleFile♦AppendEnd♦FileToModify
            if (tokens.GetCount() > 3 && Eloi.E_StringUtility.AreEquals(tokens.GetValue(1), "SingleFile", true, true))
            {
                RegexToDoubleDateFileInterpreterContainer c = new RegexToDoubleDateFileInterpreterContainer();
                c.m_regexToLookFor = tokens.GetValue(0).Trim();
                c.m_targetFilePath = tokens.GetValue(3).Trim();
                if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(2), "AppendEnd", true, true))
                    c.m_exportType = StringToExportFileType.AppendEnd;
                else if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(2), "AppendStart", true, true))
                    c.m_exportType = StringToExportFileType.AppendStart;
                else if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(2), "Override", true, true))
                    c.m_exportType = StringToExportFileType.Override;
                m_register.m_doubleInterpreter.Add(c);
            }
            //DiscordChatBot:♦DoubleFile♦AppendEnd♦OnChangeFilePath♦FileToModify
            //DallE:♦DoubleFile♦AppendEnd♦OnChangeFilePath♦FileToModify
            else if (tokens.GetCount() > 3 && Eloi.E_StringUtility.AreEquals(tokens.GetValue(1), "DoubleFile", true, true))
            {
                RegexToDoubleDateFileInterpreterContainer c = new RegexToDoubleDateFileInterpreterContainer();
                c.m_regexToLookFor = tokens.GetValue(0).Trim();
                c.m_targetFilePath = tokens.GetValue(3).Trim();
                c.m_changeHappenFilePath = tokens.GetValue(4).Trim();
                if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(2), "AppendEnd", true, true))
                    c.m_exportType = StringToExportFileType.AppendEnd;
                else if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(2), "AppendStart", true, true))
                    c.m_exportType = StringToExportFileType.AppendStart;
                else if (Eloi.E_StringUtility.AreEquals(tokens.GetValue(2), "Override", true, true))
                    c.m_exportType = StringToExportFileType.Override;
                m_register.m_doubleInterpreter.Add(c);
            }
            //Admin:♦Folder♦FolderPath
            else if (tokens.GetCount() >2 && 
                (Eloi.E_StringUtility.AreEquals(tokens.GetValue(1), "Folder", true, true) ||
                Eloi.E_StringUtility.AreEquals(tokens.GetValue(1), "Directory", true, true) ))
            {
                RegexToCommandAsNewFileInterpreterContainer folderInterpreter = new RegexToCommandAsNewFileInterpreterContainer();
                folderInterpreter.m_regexToLookFor = tokens.GetValue(0).Trim();
                folderInterpreter.m_whereToCreateFileFolderPath = tokens.GetValue(2).Trim();
                m_register.m_directoryInterpreter.Add(folderInterpreter);
            }
        }

    }
}