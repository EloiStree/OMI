using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PixelToMidiNoteParserMono : MonoBehaviour
{
    public string m_keyboardNameId;
    public Texture2D m_textureToParse;

    public float m_timePerPixel = 0.001f;

    public List<FakeMidiNote> m_eventFound = new List<FakeMidiNote>();

    public List<FakeMidiNoteRelativeTimeHolder> m_noteRelativeTime = new List<FakeMidiNoteRelativeTimeHolder>();
    public List<FakeMidiNoteDateTimeHolder> m_noteDateTime = new List<FakeMidiNoteDateTimeHolder>();


    public FakeMidiNoteGroupRelativeEvent m_onParseRelative;
    public FakeMidiNoteGroupDateTimeEvent m_onParseAbsolute;

    [ContextMenu("Parse")]
    public void Parse() {

        m_eventFound.Clear();
        m_noteRelativeTime.Clear();
        m_noteDateTime.Clear();
        bool isOnCurrentTrack=false;
        bool isPixelColored=false;
                bool changedHappend=false;

        List<FakeMidiNote> events=new List<FakeMidiNote>();
        for (int y = 0; y < m_textureToParse.height; y++)
        {
            byte note = (byte)(y % 128);
            byte channel = (byte)(y / 128);
            
            
            for (int x = 0; x < m_textureToParse.width; x++)
            {
                changedHappend = false;
                Color [] color = m_textureToParse.GetPixels(x, y, 1, 1);
                isPixelColored = 
                    !((color[0].r == 0 && color[0].g == 0 && color[0].b == 0)
                    || (color[0].r == 1 && color[0].g == 1 && color[0].b == 1)
                    || (color[0].r == 1 && color[0].g == 0 && color[0].b == 1));
                if (x == 0)
                {
                    isOnCurrentTrack = isPixelColored;
                    if(isPixelColored)  
                        changedHappend = true;
                }
                else {
                    if (isOnCurrentTrack != isPixelColored) {
                        changedHappend = true;
                        isOnCurrentTrack = isPixelColored;
                    }
                }
                if (changedHappend) {
                    byte velocity =(byte) Mathf.Clamp((int)(color[0].a * 127f),0,127);
                    FakeMidiNote fakeNote = new FakeMidiNote()
                    {
                        m_keyboardSource = m_keyboardNameId,
                        m_note_0to127 = note,
                        m_channel_0_16 = channel,
                        m_velocity_0to127 = velocity,
                        m_midiPressionType = isPixelColored ? MidiPressionType.IsPressing : MidiPressionType.IsReleasing
                    };
                    m_eventFound.Add(fakeNote);

                    FakeMidiNoteRelativeTimeHolder relativeNote = new FakeMidiNoteRelativeTimeHolder();
                    relativeNote.SetWith(fakeNote);
                    relativeNote.m_relativeSecondsToExecute = x * m_timePerPixel;
                    m_noteRelativeTime.Add(relativeNote);
                }
            }
        }
        DateTime now =  DateTime.Now;

        for (int i = 0; i < m_noteRelativeTime.Count; i++)
        {
            FakeMidiNoteDateTimeHolder dateTimeNote = new FakeMidiNoteDateTimeHolder();
            dateTimeNote.SetWith(m_noteRelativeTime[i].m_note);
            dateTimeNote.m_absoluteTimeToExecute = now.AddSeconds(m_noteRelativeTime[i].m_relativeSecondsToExecute);
            m_noteDateTime.Add(dateTimeNote);

        }
        m_noteRelativeTime = m_noteRelativeTime.OrderBy(k => k.m_relativeSecondsToExecute).ToList();
        m_noteDateTime = m_noteDateTime.OrderBy(k => k.m_absoluteTimeToExecute).ToList();

        m_onParseRelative.Invoke(new FakeMidiNoteGroupRelative(m_noteRelativeTime));
        m_onParseAbsolute.Invoke(new FakeMidiNoteGroupDateTime(m_noteDateTime));



    }

}
