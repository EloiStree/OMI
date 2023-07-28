using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class FileImport_OneLineXMLToXMLInterpreter : MonoBehaviour
{

        public List<string> m_importXmlLines = new List<string>();

       public void ClearRegister()
       {
            m_importXmlLines.Clear();

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
    for (int i = 0; i < lines.Length; i++)
    {
        char[] l = lines[i].Trim().ToCharArray();
        if (l.Length > 0 && (l[0] == '#' || (l[0] == '/' && l[1] == '/')))
            continue;
            if (lines[i].Length > 0 && ValidateXml(lines[i]))
                m_importXmlLines.Add(lines[i].Trim());


    }


    }
    public bool ValidateXml(string xmlString)
    {
        try
        {
            return XDocument.Load(new StringReader(xmlString)) != null;
        }
        catch
        {
            return false;
        }
    }
}