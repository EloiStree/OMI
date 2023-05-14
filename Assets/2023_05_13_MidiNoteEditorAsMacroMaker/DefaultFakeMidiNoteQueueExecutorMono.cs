using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFakeMidiNoteQueueExecutorMono : MonoBehaviour
{
    public FakeMidiNoteWaitingToBeExecute m_queueInWaiting;
    public bool m_useUdpate=true;

    public FakeMidiNoteDateTimeEvent m_onFakeMidiEmitted;

    
    void Update()
    {
        if (m_useUdpate)
        {
            CheckForNewMidiToPush();

        }
    }

    public void CheckForNewMidiToPush()
    {
        m_queueInWaiting.FetchNoteToExecuteAndUpdateQueue(out IEnumerable<FakeMidiNoteDateTimeHolder> notes);
        foreach (var item in notes)
        {
            m_onFakeMidiEmitted.Invoke(item);
        }
    }

    public void DebugNote(FakeMidiNoteDateTimeHolder note)
    {
        note.m_note.GetMidiPression(out MidiPressionType pression);
        note.m_note.GetNoteId_0To127(out byte n);
        note.m_note.GetChannel_0To16(out byte c);
        note.m_note.GetVelocity_0_127(out byte v);
        note.m_note.GetkeyboardName(out string sk);

        Debug.Log(string.Format("Note {0} N{1} C{2} V{3} (N:{4})",
        pression == 0 ? "Off" : "On",
        n, c, v, sk));
    }
}
