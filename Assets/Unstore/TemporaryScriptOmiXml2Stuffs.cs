using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryScriptOmiXml2Stuffs : MonoBehaviour
{
    public FileImport_OmiAsXmlElements m_import;
    public ManageMidiInListenersMono m_midiInListeners;

    public void ApplyImport() {

        foreach (var item in m_import.m_midiIn)
        {
            item.GetMidiConnectionInIdName(out string midiName);
            m_midiInListeners.StartListeningTo(midiName);
        }
    }
}
