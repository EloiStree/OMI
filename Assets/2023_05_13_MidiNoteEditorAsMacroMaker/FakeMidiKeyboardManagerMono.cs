using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FakeMidiKeyboardManagerMono : MonoBehaviour
{


    public void PlayNote(string keyboardName, int note, int channel, int velocity, bool setAsPressing) { 
    

    }

    public class NoteEmittedEvent : UnityEvent<IMidiNoteEventGet>{ 
    
    }
}

public class FakeMidiKeyboard {

    public string m_keyboardName;


}
