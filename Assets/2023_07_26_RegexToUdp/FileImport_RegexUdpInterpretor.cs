using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class FileImport_RegexUdpInterpretor : MonoBehaviour
{

    public UnityEvent m_onClearRequest;
    public RegexToUdpInterpreterContainerEvent m_onContainerfound;
    public void ClearRegister()
    {
        m_onClearRequest.Invoke();
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

            if (tokens.GetCount() == 2)
            {
                string regex = tokens.GetValue(0).Trim();
                string target = tokens.GetValue(1).Trim();
                RegexToUdpInterpreterContainer container = new RegexToUdpInterpreterContainer(regex, target);
                m_onContainerfound.Invoke(container);
            }
            else if (tokens.GetCount() == 3)
            {
                string t1 = tokens.GetValue(1).Trim().ToLower();

                string regex = tokens.GetValue(0).Trim();
                string target = tokens.GetValue(2).Trim();
                RegexToUdpInterpreterContainer container = new RegexToUdpInterpreterContainer(regex, target );


                if (t1 == "utf8" || t1 == "uft")
                {
                    container.m_exportType = RegexToUdpInterpreterContainer.ExportType.UTF8;
                }
                else if (t1 == "unicode" )
                {
                    container.m_exportType = RegexToUdpInterpreterContainer.ExportType.Unicode;
                }
                    m_onContainerfound.Invoke(container);
                
            }
        }
    }
}