
using System;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;
using UnityEngine.Events;

public class DryWetPlayerMidiOutMono : MonoBehaviour
{
    public string OutputDeviceName = "Microsoft GS Wavetable Synth";

    private OutputDevice _outputDevice;

    public string[] m_devices;

    private void Start()
    {
        InitializeOutputDevice();
    }

    private void OnApplicationQuit()
    {
        if (_outputDevice != null)
            _outputDevice.Dispose();
    }

    public bool m_found;
    private void InitializeOutputDevice()
    {
        m_found = false;
        var allOutputDevices = OutputDevice.GetAll();
        m_devices = allOutputDevices.Select(d => $"{d.Name}").ToArray();
        if (!allOutputDevices.Any(d => d.Name == OutputDeviceName))
        {
            var allDevicesList = string.Join(Environment.NewLine, allOutputDevices.Select(d => $"  {d.Name}"));
            return;
        }
        m_found = true;
        _outputDevice = OutputDevice.GetByName(OutputDeviceName);
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
            no.NoteNumber = new SevenBitNumber(n);
            no.Velocity = new SevenBitNumber(v);
            _outputDevice.SendEvent(no);
        }
        if (pression == MidiPressionType.IsReleasing)
        {
            NoteOffEvent no = new NoteOffEvent();
            no.Channel = new FourBitNumber(c);
            no.NoteNumber = new SevenBitNumber(n);
            no.Velocity = new SevenBitNumber(v);
            _outputDevice.SendEvent(no);
        }
    }
}