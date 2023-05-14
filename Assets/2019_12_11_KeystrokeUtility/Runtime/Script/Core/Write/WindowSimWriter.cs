using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput;
using WindowsInput.Native;

public class WindowSimWriter : KeyboardWriteMono
{

    InputSimulator m_winKey;
    public void Awake()
    {
        m_winKey = new InputSimulator();
    }


    public void WriteText(string text) {
        m_winKey.Keyboard.TextEntry(text);
    }

    public void MouseLeftButtonDown() =>
        m_winKey.Mouse.LeftButtonDown();
    public void MouseLeftButtonUp() =>
            m_winKey.Mouse.LeftButtonUp();

    public void MouseRightButtonDown() =>
        m_winKey.Mouse.RightButtonDown();
    public void MouseRightButtonUp() =>
            m_winKey.Mouse.RightButtonUp();


    public void MouseMiddleButtonDown() =>
     NativeMouseKeyStroke.MiddleClick(true);
//        m_winKey.Mouse.XButtonDown(3);//TO VERIFIED

    public void MouseMiddleButtonUp() => 
     NativeMouseKeyStroke.MiddleClick(false);
    //      m_winKey.Mouse.XButtonUp(3);//TO VERIFIED



    public override void RealPressDown(KeyboardTouch touch)
    {
        bool isConvertable;
        VirtualKeyCode vk;
        KeyBindingTable.ConvertTouchToWindowVirtualKeyCodes(touch, out vk, out isConvertable);
        if (isConvertable) {
            if (vk == VirtualKeyCode.LBUTTON) MouseLeftButtonDown();
            else if (vk == VirtualKeyCode.RBUTTON) MouseRightButtonDown();
            else if (vk == VirtualKeyCode.MBUTTON) MouseMiddleButtonDown();
            else 
            m_winKey.Keyboard.KeyDown(vk);
        }
    }

    public override void RealPressUp(KeyboardTouch touch)
    {
        bool isConvertable;
        VirtualKeyCode vk;
        KeyBindingTable.ConvertTouchToWindowVirtualKeyCodes(touch, out vk, out isConvertable);
        if (isConvertable) {
         
            if (vk == VirtualKeyCode.LBUTTON) MouseLeftButtonUp();
            else if (vk == VirtualKeyCode.RBUTTON) MouseRightButtonUp();
            else if (vk == VirtualKeyCode.MBUTTON) MouseMiddleButtonUp();
            else
                m_winKey.Keyboard.KeyUp(vk);
        }
    }
    public  void VirtualKeyStroke(VirtualKeyCode key, PressType pressType)
    {
        if (pressType == PressType.Down || pressType == PressType.Both) { 
            if (key == VirtualKeyCode.LBUTTON) MouseLeftButtonDown();
            else if (key == VirtualKeyCode.RBUTTON) MouseRightButtonDown();
            else if (key == VirtualKeyCode.MBUTTON) MouseMiddleButtonDown();
            else
                m_winKey.Keyboard.KeyDown(key);
        }
        if (pressType == PressType.Up || pressType == PressType.Both) { 
            if (key == VirtualKeyCode.LBUTTON) MouseLeftButtonUp();
            else if (key == VirtualKeyCode.RBUTTON) MouseRightButtonUp();
            else if (key == VirtualKeyCode.MBUTTON) MouseMiddleButtonUp();
            else

                m_winKey.Keyboard.KeyUp(key);
        }

    }

    public override void Stroke(string text)
    {
        
            m_winKey.Keyboard.TextEntry(text);

    }

    public override void Stroke(char character)
    {
        m_winKey.Keyboard.TextEntry(character.ToString());
    }

    public override void Stroke(KeyboardTouchPressRequest key)
    {
        if (key.m_pression == PressType.Down || key.m_pression == PressType.Both)
            RealPressDown(key.m_touch);
        if (key.m_pression == PressType.Up || key.m_pression == PressType.Both)
            RealPressUp(key.m_touch);
    }

    public override void Stroke(KeyboardCharPressRequest character)
    {
        Stroke(character.m_character);
    }

    public override void Stroke(AsciiStrokeRequest asciiCode)
    {
        char c; bool ic;
        KeyBindingTable.ConvertASCIIToChar(asciiCode.m_code, out  c, out ic);
        Stroke(c);
    }

    public override void Stroke(UnicodeStrokeRequest unicode)
    {
        char c; bool ic;
        KeyBindingTable.ConvertUnicodeToChar(unicode.m_code, out c, out ic);
        Stroke(c);
    }

    public class NativeMouseKeyStroke {

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy,
                      int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }


        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        public static void MiddleClick(bool useDown)
        {
            POINT p = GetCursorPosition();
            if (useDown)
                mouse_event((int)(MouseEventFlags.MIDDLEDOWN), p.X, p.Y, 0, 0);
            else
                mouse_event((int)(MouseEventFlags.MIDDLEUP), p.X, p.Y, 0, 0);
        }
        public static void LeftClick(bool useDown)
        {
            POINT p = GetCursorPosition();
            if (useDown)

                mouse_event((int)(MouseEventFlags.LEFTDOWN), p.X, p.Y, 0, 0);
            else
                mouse_event((int)(MouseEventFlags.LEFTUP), p.X, p.Y, 0, 0);
        }
        public static void RightClick(bool useDown)
        {
            POINT p = GetCursorPosition();
            if (useDown)

                mouse_event((int)(MouseEventFlags.RIGHTDOWN), p.X, p.Y, 0, 0);
            else mouse_event((int)(MouseEventFlags.RIGHTUP), p.X, p.Y, 0, 0);
        }


    }
}
