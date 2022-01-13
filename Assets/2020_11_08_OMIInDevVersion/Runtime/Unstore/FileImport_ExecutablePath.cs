using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileImport_ExecutablePath : MonoBehaviour
{
    public ExecutalePathRegister m_register;
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
                if (filePath[i].ToLower().EndsWith(".launchablepath"))
                    PushLaunchableFileInfo(File.ReadAllText(filePath[i]));

                else
                    m_register.AddFromPath(filePath[i], "");
            }
        }
    }

    private void PushLaunchableFileInfo(string textToLoad)
    {
        string[] lines = textToLoad.Split('\n');
        TileLine tokens;
        for (int i = 0; i < lines.Length; i++)
        {
            char[] l = lines[i].Trim().ToCharArray();
            if (l.Length > 0 && (l[0] == '#' || (l[0] == '/' && l[1] == '/')))
                continue;

            tokens = new TileLine(lines[i]);
            //MPK Mini Play♦Shorten♦6321♦FootSustain♦SetFalse

            if (tokens.GetCount() == 1)
            {
                if(tokens.GetValue(0).Trim().Length>0)
                    m_register.AddFromPath(tokens.GetValue(0), "");
            }
            if (tokens.GetCount() == 2)
            {
                if (tokens.GetValue(0).Trim().Length > 0 && tokens.GetValue(1).Trim().Length > 0)
                    m_register.AddFromPath( tokens.GetValue(1).Trim(), tokens.GetValue(0).Trim());
            }
            if (tokens.GetCount() > 2)
            {
                List<string> s = tokens.GetAsList();
                s.RemoveAt(0);    s.RemoveAt(0);
                if (tokens.GetValue(0).Trim().Length > 0 && tokens.GetValue(1).Trim().Length > 0)
                    m_register.AddFromPath(tokens.GetValue(1).Trim(), tokens.GetValue(0).Trim(), s);
            }



        }
    }
}

