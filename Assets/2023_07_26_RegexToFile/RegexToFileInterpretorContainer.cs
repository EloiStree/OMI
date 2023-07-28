using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StringToExportFileType { Override, AppendStart, AppendEnd }

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class RegexToDoubleDateFileInterpreterContainer 
{
    public string m_regexToLookFor="";
    public string m_targetFilePath="";
    public string m_changeHappenFilePath="";
    public StringToExportFileType m_exportType = StringToExportFileType.AppendEnd;
}


/// <summary>
/// If the regex is match, the command is store in generated file in the target folder to be handle by an other third application.
/// </summary>
[System.Serializable]
public class RegexToCommandAsNewFileInterpreterContainer
{
    public string m_regexToLookFor="";
    public string m_whereToCreateFileFolderPath="";
}