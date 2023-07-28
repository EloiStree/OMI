using System;
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

public class DemoScript : MonoBehaviour
{
    private const string OutputDeviceName = "Microsoft GS Wavetable Synth";

    public string m_midiPath;

    private OutputDevice _outputDevice;
    private Playback _playback;



    public MusicOverrideAsMacro[] m_musicOverrides;

    public Eloi.PrimitiveUnityEvent_String m_onStringEmitted;

    [System.Serializable]
    public class MusicOverrideAsMacro {
        public bool m_playMusic;
        public int m_channel;
        public string m_noteAsString;
        public int m_noteAsMidiInt;
        public string[] m_onPress;
        public string[] m_onRelease;
    }

   


    private void Start()
    {
        InitializeOutputDevice();
       // var midiFile = CreateTestFile();
            var midiFile = MidiFile.Read(m_midiPath);
        InitializeFilePlayback(midiFile);
        StartPlayback();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Releasing playback and device...");

        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.NotesPlaybackFinished -= OnNotesPlaybackFinished;
            _playback.Dispose();
        }

        if (_outputDevice != null)
            _outputDevice.Dispose();

        Debug.Log("Playback and device released.");
    }

    private void InitializeOutputDevice()
    {
        Debug.Log($"Initializing output device [{OutputDeviceName}]...");

        var allOutputDevices = OutputDevice.GetAll();
        if (!allOutputDevices.Any(d => d.Name == OutputDeviceName))
        {
            var allDevicesList = string.Join(Environment.NewLine, allOutputDevices.Select(d => $"  {d.Name}"));
            Debug.Log($"There is no [{OutputDeviceName}] device presented in the system. Here the list of all device:{Environment.NewLine}{allDevicesList}");
            return;
        }

        _outputDevice = OutputDevice.GetByName(OutputDeviceName);
        Debug.Log($"Output device [{OutputDeviceName}] initialized.");
    }

    private MidiFile CreateTestFile()
    {
        Debug.Log("Creating test MIDI file...");

        var patternBuilder = new PatternBuilder()
            .SetNoteLength(MusicalTimeSpan.Eighth)
            .SetVelocity(SevenBitNumber.MaxValue)
            .ProgramChange(GeneralMidiProgram.Harpsichord);

        foreach (var noteNumber in SevenBitNumber.Values)
        {
            patternBuilder.Note(Melanchall.DryWetMidi.MusicTheory.Note.Get(noteNumber));
        }

        var midiFile = patternBuilder.Build().ToFile(TempoMap.Default);
        Debug.Log("Test MIDI file created.");

        return midiFile;
    }

    private void InitializeFilePlayback(MidiFile midiFile)
    {
        Debug.Log("Initializing playback...");

        _playback = midiFile.GetPlayback(_outputDevice);
        _playback.Loop = false;
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        _playback.NotesPlaybackFinished += OnNotesPlaybackFinished;
       
        Debug.Log("Playback initialized.");
    }

    private void StartPlayback()
    {
        Debug.Log("Starting playback...");
        _playback.Start();
    }

    private void OnNotesPlaybackFinished(object sender, NotesEventArgs e)
    {
        LogNotes("Notes finished:", e);
    }

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        LogNotes("Notes started:", e);
    }

    private void LogNotes(string title, NotesEventArgs e)
    {
        var message = new StringBuilder()
            .AppendLine(title)
            .AppendLine(string.Join(Environment.NewLine, e.Notes.Select(n => $"  {n}")))
            .ToString();
        foreach (var item in e.Notes)
        {
            message += " - C:" + item.Channel;
        }

        Debug.Log(message.Trim());
    }

    public void PlayNote(FakeMidiNoteDateTimeHolder note)
    {
        note.m_note.GetMidiPression(out MidiPressionType pression);
        note.m_note.GetNoteId_0To127(out byte n);
        note.m_note.GetChannel_0To16(out byte c);
        note.m_note.GetVelocity_0_127(out byte v);
        note.m_note.GetkeyboardName(out string sk);

        //Debug.Log(string.Format("Note {0} N{1} C{2} V{3} (N:{4})",
        //pression == 0 ? "Off" : "On",
        //n, c, v, sk));

        if (pression == MidiPressionType.IsPressing)
        {
            NoteOnEvent no = new NoteOnEvent();
            no.Channel = new FourBitNumber(c);
            no.NoteNumber = new SevenBitNumber((byte)n);
            no.Velocity = new SevenBitNumber((byte)v);
           
            _outputDevice.SendEvent(no);
        }
        if (pression == MidiPressionType.IsReleasing)
        {
            NoteOffEvent no = new NoteOffEvent();
            no.Channel = new FourBitNumber(c);
            no.NoteNumber = new SevenBitNumber((byte)n);
            no.Velocity = new SevenBitNumber((byte)v);
            _outputDevice.SendEvent(no);
        }
    }
}