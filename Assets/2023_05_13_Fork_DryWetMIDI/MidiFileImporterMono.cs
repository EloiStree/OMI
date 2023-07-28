
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;
using UnityEngine.Events;

public class MidiFileImporterMono : MonoBehaviour
{



    public FakeMidiNoteGroupRelativeEvent m_onParseRelative;
    [TextArea(0, 4)]
    public string m_midiPath;

    public double m_multiplicationFactor=1;

    [ContextMenu("Import File")]
    public void ImportMidiFile()
    {

        //https://www.recordingblogs.com/wiki/time-division-of-a-midi-file
        //1 tick = microseconds per beat / 60
        //1 tick = 1,000,000 / (24 * 100) = 416.66 microseconds

        List<FakeMidiNoteRelativeTimeHolder> midi = new List<FakeMidiNoteRelativeTimeHolder>();
        var midiFile = MidiFile.Read(m_midiPath);
        TimeSpan midiFileDuration = midiFile.GetDuration<MetricTimeSpan>();

        TempoMap tempoMap = midiFile.GetTempoMap();
       
        foreach (var item in midiFile.GetNotes())
        {
            MetricTimeSpan start = item.TimeAs<MetricTimeSpan>(tempoMap);
            MetricTimeSpan end = item.EndTimeAs<MetricTimeSpan>(tempoMap);
            FakeMidiNoteRelativeTimeHolder n = new FakeMidiNoteRelativeTimeHolder();
            n.m_relativeSecondsToExecute = start.TotalSeconds * m_multiplicationFactor;
            //n.m_relativeSecondsToExecute = item.GetTimedNoteOnEvent().TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;
           
            n.m_note.m_note_0to127 = item.NoteNumber;
            n.m_note.m_velocity_0to127 = item.Velocity;
            n.m_note.m_channel_0_16 = item.Channel;
            n.m_note.m_midiPressionType = MidiPressionType.IsPressing;
            midi.Add(n);

            FakeMidiNoteRelativeTimeHolder ne = new FakeMidiNoteRelativeTimeHolder();
             ne.m_relativeSecondsToExecute = end.TotalSeconds * m_multiplicationFactor;
            //ne.m_relativeSecondsToExecute = item.GetTimedNoteOffEvent().TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;
            ne.m_note.m_note_0to127 = item.NoteNumber;
            ne.m_note.m_velocity_0to127 = item.Velocity;
            ne.m_note.m_channel_0_16 = item.Channel;
            ne.m_note.m_midiPressionType = MidiPressionType.IsReleasing;
            midi.Add(ne);

        }
        m_onParseRelative.Invoke(new FakeMidiNoteGroupRelative(midi));
    }

   



}