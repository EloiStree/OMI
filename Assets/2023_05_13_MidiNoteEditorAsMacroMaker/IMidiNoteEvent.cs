using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public enum MidiPressionType:int { IsPressing=1, IsReleasing=0}
public interface IFakeMidiNoteEvent
{

    public void GetkeyboardName(out string keyboardName);
    public void GetChannel_0To16(out byte channel);
    public void GetNoteId_0To127(out byte note);
    public void GetVelocity_0_127(out byte velocity);
    public void GetMidiPression(out MidiPressionType pressionType);
}

public interface IFakeMidiNoteHolder {
    public void GetMidiNote(out IFakeMidiNoteEvent note);
}
public interface IFakeMidiNoteRelativeTimeHolder : IFakeMidiNoteHolder
{
    public void GetWhenToExecuteInRelativeSeconds(out double relativeSeconds);
}
public interface IFakeMidiNoteDateTimeHolder : IFakeMidiNoteHolder
{
    public void GetWhenToExecuteInDateTime(out DateTime whenToExecute);
}


public class FakeMidiNoteUtility {
    public static void ConvertRelativeNotetToDateTimeNote(
        IEnumerable<FakeMidiNoteRelativeTimeHolder> source,
        DateTime startTime,
        out IEnumerable<FakeMidiNoteDateTimeHolder> dateTimeResult
        ) {
        List<FakeMidiNoteDateTimeHolder> result = new List<FakeMidiNoteDateTimeHolder>();
        DateTime now = DateTime.Now;

        foreach (FakeMidiNoteRelativeTimeHolder item in source)
        {
            FakeMidiNoteDateTimeHolder dateTimeNote = new FakeMidiNoteDateTimeHolder();
            dateTimeNote.SetWith(item.m_note);
            dateTimeNote.m_absoluteTimeToExecute = now.AddSeconds(item.m_relativeSecondsToExecute);
            result.Add(dateTimeNote);
        }
        result = result.OrderBy(k => k.m_absoluteTimeToExecute).ToList();
        dateTimeResult = result;

    }

}

[System.Serializable]
public class FakeMidiNoteGroupRelativeEvent : UnityEvent<FakeMidiNoteGroupRelative> { }
[System.Serializable]
public class FakeMidiNoteGroupDateTimeEvent : UnityEvent<FakeMidiNoteGroupDateTime> { }
[System.Serializable]
public class FakeMidiNoteDateTimeEvent : UnityEvent<FakeMidiNoteDateTimeHolder> { }
[System.Serializable]
public class FakeMidiNoteRelativeEvent : UnityEvent<FakeMidiNoteRelativeTimeHolder> { }

[System.Serializable]
public struct FakeMidiNoteGroupRelative
{
    public FakeMidiNoteRelativeTimeHolder[] m_notesInOrder;

    public FakeMidiNoteGroupRelative(IEnumerable<FakeMidiNoteRelativeTimeHolder> note)
    {
        m_notesInOrder = note.OrderBy(k => k.m_relativeSecondsToExecute).ToArray();
    }
}
[System.Serializable]
public struct FakeMidiNoteGroupDateTime
{
    public FakeMidiNoteDateTimeHolder[] m_notesInOrder;
    public FakeMidiNoteGroupDateTime(IEnumerable<FakeMidiNoteDateTimeHolder> note)
    {
        m_notesInOrder = note.OrderBy(k => k.m_absoluteTimeToExecute).ToArray();
    }
}

[System.Serializable]
public struct FakeMidiNoteRelativeTimeHolder : IFakeMidiNoteRelativeTimeHolder
{
    public double m_relativeSecondsToExecute;
    public FakeMidiNote m_note;

    public void SetWith(IFakeMidiNoteEvent note) {
        m_note.SetWith(note);
    }
    public void GetMidiNote(out IFakeMidiNoteEvent note)
    {
        note = m_note;
    }

    public void GetWhenToExecuteInRelativeSeconds(out double relativeSeconds)
    {
        relativeSeconds = m_relativeSecondsToExecute;
    }
}
[System.Serializable]
public struct FakeMidiNoteDateTimeHolder : IFakeMidiNoteDateTimeHolder
{

    public DateTime m_absoluteTimeToExecute;
    public FakeMidiNote m_note;

    public void SetWith(IFakeMidiNoteEvent note)
    {
        m_note.SetWith(note);
    }
    public void GetMidiNote(out IFakeMidiNoteEvent note)
    {
        note = m_note;
    }

    public void GetWhenToExecuteInDateTime(out DateTime whenToExecute )
    {
        whenToExecute= m_absoluteTimeToExecute ;
    }
}


[System.Serializable]
public struct FakeMidiNote : IFakeMidiNoteEvent {
    public string m_keyboardSource;
    public byte m_channel_0_16;
    public byte m_note_0to127;
    public byte m_velocity_0to127;
    public MidiPressionType m_midiPressionType;

    public void GetChannel_0To16(out byte channel) =>
        channel = m_channel_0_16;

    public void GetkeyboardName(out string keyboardName) =>
        keyboardName = m_keyboardSource;
    public void GetMidiPression(out MidiPressionType pressionType) =>
        pressionType = m_midiPressionType;

    public void GetNoteId_0To127(out byte note) =>
        note = m_note_0to127;

    public void GetVelocity_0_127(out byte velocity) =>
        velocity = m_velocity_0to127;

    public void SetWith(IFakeMidiNoteEvent note)
    {
        note.GetChannel_0To16(out m_channel_0_16);
        note.GetMidiPression(out m_midiPressionType);
        note.GetNoteId_0To127(out m_note_0to127 );
        note.GetVelocity_0_127(out m_velocity_0to127);
        note.GetkeyboardName(out m_keyboardSource);
    }
}