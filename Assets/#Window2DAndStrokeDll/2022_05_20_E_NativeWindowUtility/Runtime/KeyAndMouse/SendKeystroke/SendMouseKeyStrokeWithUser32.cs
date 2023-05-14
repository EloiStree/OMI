using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SendMouseKeyStrokeWithUser32 
{
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
