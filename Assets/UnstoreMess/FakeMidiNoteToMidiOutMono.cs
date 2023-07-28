using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using NAudio.Midi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class FakeMidiNoteToMidiOutMono : MonoBehaviour
{
   
    public  MidiOutMono m_connectionMono;



    public void SendNote()
    {
        if (m_connectionMono.IsUsable())
        {
          
        }
    }
    public void SendNoteOnOff(bool isOn)
    {
        if (m_connectionMono.IsUsable())
        {
            MidiCommandCode cmd = isOn ? MidiCommandCode.NoteOn : MidiCommandCode.NoteOff;
        }
    }
    public void PlayNote(FakeMidiNoteDateTimeHolder note)
    {
        if (!m_connectionMono.IsUsable())
            return;
            note.m_note.GetMidiPression(out MidiPressionType pression);
        note.m_note.GetNoteId_0To127(out byte n);
        note.m_note.GetChannel_0To16(out byte c);
        note.m_note.GetVelocity_0_127(out byte v);
        note.m_note.GetkeyboardName(out string sk);

        if (pression == MidiPressionType.IsPressing)
        {
            var noteOnEvent = new NAudio.Midi.NoteOnEvent(0, c+1, n, v,200);
            m_connectionMono.SendCommandAsRawInt(noteOnEvent.GetAsShortMessage());
        }
        if (pression == MidiPressionType.IsReleasing)
        {
            var noteOnEvent = new NAudio.Midi.NoteEvent(0, c + 1, MidiCommandCode.NoteOff, n, v);
            m_connectionMono.SendCommandAsRawInt(noteOnEvent.GetAsShortMessage());
        }
    }
}