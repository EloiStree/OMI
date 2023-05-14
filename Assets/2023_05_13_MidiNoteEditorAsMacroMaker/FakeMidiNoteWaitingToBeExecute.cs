using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FakeMidiNoteWaitingToBeExecute : MonoBehaviour
{

    public List<FakeMidiNoteDateTimeHolder> m_notes = new List<FakeMidiNoteDateTimeHolder>();

    public DateTime m_previous;
    public DateTime m_current;


    private void Start()
    {
        m_previous=DateTime.Now;
        m_current=DateTime.Now;
    }

    public void FetchNoteToExecuteAndUpdateQueue(out IEnumerable<FakeMidiNoteDateTimeHolder> toPlayFound)
    {
        m_current=( DateTime.Now);
        toPlayFound = m_notes
            .Where(k => k.m_absoluteTimeToExecute <= m_current && k.m_absoluteTimeToExecute > m_previous)
            .OrderBy(k => k.m_absoluteTimeToExecute)
            .ToList();
        
        m_notes = m_notes
            .Where(k => k.m_absoluteTimeToExecute > m_current).ToList();
        m_previous=( m_current);
    }

    public void PushNote(FakeMidiNoteGroupRelative group) {
        FakeMidiNoteUtility.ConvertRelativeNotetToDateTimeNote(group.m_notesInOrder, DateTime.Now,
            out IEnumerable<FakeMidiNoteDateTimeHolder> notes);
        m_notes.AddRange(notes);
    }
    public void PushNote(FakeMidiNoteGroupDateTime group)
    {
        m_notes.AddRange(group.m_notesInOrder);
    }
    public void PushNote(IEnumerable< FakeMidiNoteRelativeTimeHolder>  group)
    {
        FakeMidiNoteUtility.ConvertRelativeNotetToDateTimeNote(group, DateTime.Now,
            out IEnumerable<FakeMidiNoteDateTimeHolder> notes);
        m_notes.AddRange(notes);

    }
    public void PushNote(IEnumerable<FakeMidiNoteDateTimeHolder> notes)
    {
        m_notes.AddRange(notes);
    }
}
