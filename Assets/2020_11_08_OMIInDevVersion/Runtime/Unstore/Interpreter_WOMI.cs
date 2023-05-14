using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput.Native;

public class Interpreter_WOMI : AbstractInterpreterMono
{
    public WindowSimWriter m_keyWriter;
    private void Awake()
    {}


    public override bool CanInterpreterUnderstand(ref ICommandLine command)
    {
        return StartWith(ref command, "winkey:", true);
    }

    public override string GetName()
    {
        return "Window Keyboard Input";
    }

    public override void TranslateToActionsWithStatus(ref ICommandLine command, ref ExecutionStatus succedToExecute)
    {
        string cmd = command.GetLine().Trim();
        string cmdLow = command.GetLine().Trim().ToLower();
        string[] token = cmd.Split(':');
 
  //      Debug.Log("HEY mon ami||" + cmd);
        if (token.Length >= 3)
        {
            string toPress = token[2];
            string pressTypeString = token[1].ToLower().Trim();
            VirtualKeyCode key ;
            bool found;
            ConvertStringToEnum(toPress, out found, out key);
            PressType pressType= PressType.Both;
            bool doubleClick= pressTypeString.IndexOf("double")==0;


            if (cmdLow.IndexOf("winkey:clipboard:") == 0)
            {
                string t = cmd.Substring("winkey:clipboard:".Length);
                UnityClipboard.Set(t);
                //SetClipboardUser32(t);

            }
            if (cmdLow.IndexOf("winkey:write:") == 0)
            {
                string t = cmd.Substring("winkey:write:".Length);
                m_keyWriter.WriteText(t);
            }

            if (pressTypeString.IndexOf("press") == 0 || pressTypeString.IndexOf("p") == 0)
            {
                pressType = PressType.Down;
            }
            else if (pressTypeString.IndexOf("release") == 0 || pressTypeString.IndexOf("r") == 0)
            {
                pressType = PressType.Up;
            }
            else if (pressTypeString.IndexOf("stroke") == 0 || pressTypeString.IndexOf("s") == 0)
            {
                pressType = PressType.Both;
            }
            


            if (found) {
                if (doubleClick) { 
                    m_keyWriter.VirtualKeyStroke(key, PressType.Both);
                    m_keyWriter.VirtualKeyStroke(key, PressType.Both);
                }
                else
                    m_keyWriter.VirtualKeyStroke(key, pressType);

            }
        }
        
        succedToExecute.SetAsFinished(true);
    }


    [DllImport("user32.dll")]
    internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    internal static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

    private void SetClipboardUser32(string t)
    {
        OpenClipboard(IntPtr.Zero);
        var ptr = Marshal.StringToHGlobalUni(t);
        SetClipboardData(13, ptr);
        CloseClipboard();
        Marshal.FreeHGlobal(ptr);
    }

    private void ConvertStringToEnum(string toPress, out bool found, out VirtualKeyCode key)
    {
        toPress=toPress.ToLower().Trim();

        toPress = toPress.Replace("enter", "return");
        CheckForClassicAlphaNumPlus(toPress, out  found, out key);
        if (found)
            return;

        for (int i = 0; i < m_keysLow.Length; i++)
        {
            if (m_keysLow[i].keyAsString == toPress) {
                found = true;
                key = m_keysLow[i].key;
                return;
            }
        }
        

        found = false;
        key = VirtualKeyCode.NONCONVERT;
    }

    private void CheckForClassicAlphaNumPlus(string toPress, out bool found, out VirtualKeyCode key)
    {
        found = false;
        key = VirtualKeyCode.NONCONVERT;
        toPress = toPress.ToUpper().Trim();
        if (toPress.Length == 0)
        {
            return;
        }
        char c = toPress.ToUpper()[0];
        switch (c)
        {

            case 'N':
                if (toPress.Length == 1)
                {
                    found = true; key = VirtualKeyCode.VK_N;
                }
                else {
                    if (toPress.IndexOf("NP0")==0) { found = true; key = VirtualKeyCode.NUMPAD0; }
                    if (toPress.IndexOf("NP1")==0 ){ found = true; key = VirtualKeyCode.NUMPAD1; }
                    if (toPress.IndexOf("NP2")==0 ){ found = true; key = VirtualKeyCode.NUMPAD2; }
                    if (toPress.IndexOf("NP3")==0 ){ found = true; key = VirtualKeyCode.NUMPAD3; }
                    if (toPress.IndexOf("NP4")==0 ){ found = true; key = VirtualKeyCode.NUMPAD4; }
                    if (toPress.IndexOf("NP5")==0 ){ found = true; key = VirtualKeyCode.NUMPAD5; }
                    if (toPress.IndexOf("NP6")==0 ){ found = true; key = VirtualKeyCode.NUMPAD6; }
                    if (toPress.IndexOf("NP7")==0 ){ found = true; key = VirtualKeyCode.NUMPAD7; }
                    if (toPress.IndexOf("NP8")==0 ){ found = true; key = VirtualKeyCode.NUMPAD8; }
                    if (toPress.IndexOf("NP9") == 0) { found = true; key = VirtualKeyCode.NUMPAD9; }
                }
                break;

            case 'F': 
                if (toPress.Length == 1)
                {
                    found = true; key = VirtualKeyCode.VK_F;
                }
                else
                {
                    if (toPress == "F1") { found = true; key = VirtualKeyCode.F1; }
                    if (toPress == "F2") { found = true; key = VirtualKeyCode.F2; }
                    if (toPress == "F3") { found = true; key = VirtualKeyCode.F3; }
                    if (toPress == "F4") { found = true; key = VirtualKeyCode.F4; }
                    if (toPress == "F5") { found = true; key = VirtualKeyCode.F5; }
                    if (toPress == "F6") { found = true; key = VirtualKeyCode.F6; }
                    if (toPress == "F7") { found = true; key = VirtualKeyCode.F7; }
                    if (toPress == "F8") { found = true; key = VirtualKeyCode.F8; }
                    if (toPress == "F9") { found = true; key = VirtualKeyCode.F9; }
                    if (toPress == "F10") { found = true; key = VirtualKeyCode.F10; }
                    if (toPress == "F11") { found = true; key = VirtualKeyCode.F11; }
                    if (toPress == "F12") { found = true; key = VirtualKeyCode.F12; }
                    if (toPress == "F13") { found = true; key = VirtualKeyCode.F13; }
                    if (toPress == "F14") { found = true; key = VirtualKeyCode.F14; }
                    if (toPress == "F15") { found = true; key = VirtualKeyCode.F15; }
                    if (toPress == "F16") { found = true; key = VirtualKeyCode.F16; }
                    if (toPress == "F17") { found = true; key = VirtualKeyCode.F17; }
                    if (toPress == "F18") { found = true; key = VirtualKeyCode.F18; }
                    if (toPress == "F19") { found = true; key = VirtualKeyCode.F19; }
                    if (toPress == "F20") { found = true; key = VirtualKeyCode.F20; }
                    if (toPress == "F21") { found = true; key = VirtualKeyCode.F21; }
                    if (toPress == "F22") { found = true; key = VirtualKeyCode.F22; }
                    if (toPress == "F23") { found = true; key = VirtualKeyCode.F23; }
                    if (toPress == "F24") { found = true; key = VirtualKeyCode.F24; }
                }
                break;


            case ' ':if (toPress.Length == 1) found = true; key = VirtualKeyCode.SPACE; break;
            case '+':if (toPress.Length == 1) found = true; key = VirtualKeyCode.ADD; break;
            case '-':if (toPress.Length == 1) found = true; key = VirtualKeyCode.SUBTRACT; break;
            case '/':if (toPress.Length == 1) found = true; key = VirtualKeyCode.DIVIDE; break;
            case '*':if (toPress.Length == 1) found = true; key = VirtualKeyCode.MULTIPLY; break;
            case '.':if (toPress.Length == 1) found = true; key = VirtualKeyCode.DECIMAL; break;
            case '0':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_0; break;
            case '1':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_1; break;
            case '2':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_2; break;
            case '3':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_3; break;
            case '4':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_4; break;
            case '5':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_5; break;
            case '6':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_6; break;
            case '7':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_7; break;
            case '8':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_8; break;
            case '9':if (toPress.Length == 1) found = true; key = VirtualKeyCode.VK_9; break;


case 'A': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_A;  break;
case 'B': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_B ;break;
case 'C': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_C ;break;
case 'D': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_D ;break;
case 'E': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_E ;break;
case 'G': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_G ;break;
case 'H': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_H ;break;
case 'I': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_I ;break;
case 'J': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_J ;break;
case 'K': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_K ;break;
case 'L': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_L ;break;
case 'M': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_M ;break;
case 'O': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_O ;break;
case 'P': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_P ;break;
case 'Q': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_Q ;break;
case 'R': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_R ;break;
case 'S': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_S ;break;
case 'T': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_T ;break;
case 'U': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_U ;break;
case 'V': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_V ;break;
case 'W': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_W ;break;
case 'X': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_X ;break;
case 'Y': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_Y ;break;
case 'Z': if (toPress.Length == 1) found = true; key = VirtualKeyCode. VK_Z ;break;
            default:
                break;
        }
    }

    public void Reset()
    {
        m_keys = GetListOfVirtualKey();
        m_keysLow = new KeyLow[m_keys.Length];
        List<string> tmp = new List<string>();
        for (int i = 0; i < m_keys.Length; i++)
        {
            string low = m_keys[i].ToString().ToLower();
            tmp.Add(low);
            m_keysLow[i] = new KeyLow() { keyAsString = low, key = m_keys[i] };
        }
        m_copyPastable = string.Join("\n", tmp);
    }
    [TextArea(0,5)]
    public string m_copyPastable;
    [System.Serializable]
    public class KeyLow {
        public string keyAsString;
        public VirtualKeyCode key;
    }
    public KeyLow[] m_keysLow = new KeyLow[0];
    public VirtualKeyCode[] m_keys= new VirtualKeyCode[0];
    public VirtualKeyCode[] GetListOfVirtualKey() {
      return   Enum.GetValues(typeof(VirtualKeyCode)).Cast<VirtualKeyCode>().ToArray();
    }
    public override void WhatIsYourRequirementFor(ref ICommandLine command, out ICommandExecutioninformation executionInfo)
    {
        executionInfo= new CommandExecutionInformation(false, false, false, false);
    }

    public override string WhatWillYouDoWith(ref ICommandLine command)
    {
        return "";
    }

   
}
