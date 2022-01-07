using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using System.Xml.Serialization;
using OMIAbstraction;

public class FileImport_OmiAsXmlElements : MonoBehaviour
{

    public List<string> m_filesPathFound = new List<string>();
    public List<Omixml> m_xmlFoundAndValide= new List<Omixml>();

    public List<MidiConnectionIn> m_midiIn;
    public List<MidiConnectionOut> m_midiOut;
    public void Clear() {
        m_filesPathFound.Clear();
    }

    public void TryToImportAllFiles(string[] filesPath)
    {
        for (int i = 0; i < filesPath.Length; i++)
        {
            string p = filesPath[i];
            TryToImportAllFile(p);

        }

    }
    public void TryToImportAllFile(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        m_filesPathFound.Add(filePath);
        string text  = File.ReadAllText(filePath);
      
         XmlSerializer serializer = new XmlSerializer(typeof(Omixml));
         using (StringReader reader = new StringReader(text))
         {
            Omixml omiXml = (Omixml)serializer.Deserialize(reader);
            m_xmlFoundAndValide.Add(omiXml);
            foreach (var item in omiXml.Midiconnectionin)
            {
                m_midiIn.Add(new MidiConnectionIn(item.MidiName));
            }
            foreach (var item in omiXml.Midiconnectionout)
            {
                m_midiOut.Add(new MidiConnectionOut(item.MidiName));
            }
        }

    }
}
