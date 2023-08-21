using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringToFileTempMono : MonoBehaviour
{
    public Eloi.AbstractMetaAbsolutePathFileMono m_whereToCreateFile;
    public void PushText(string text)
    {
        if (m_whereToCreateFile) {

            Eloi.E_FileAndFolderUtility.CreateFolderIfNotThere(m_whereToCreateFile);
            Eloi.E_FileAndFolderUtility.ExportByOverriding(m_whereToCreateFile, text);
           // Debug.Log("SAVE FULL LOG:"+ m_whereToCreateFile.GetPath());
        }
    }
}
