using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;

public class FileImport_BlockLineXMLToXMLInterpreter : MonoBehaviour
{

    public List<string> m_importXmlBlockLines = new List<string>();

    public void ClearRegister()
    {
        m_importXmlBlockLines.Clear();
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
    public string m_lineSpliterChar= "?ù";
    private void ImportFileText(string text)
    {
        string [] textBlock = Regex.Split(text, "([" + m_lineSpliterChar + "])+([\\s\\r\\n])+");
        m_importXmlBlockLines.AddRange(textBlock.Where(k => k.Trim().Length > 0));
    }

    //public bool IsLineSpliter(string line) {
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        if (!(line[i] == ' ' || line[i] == m_lineSpliterChar || line[i] == '\n' || line[i] == '\r'))
    //            return false;
    //    }
    //    return true;

    //}

    public bool Valida(string xmlString)
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