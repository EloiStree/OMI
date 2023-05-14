
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class WindowIntPtrUtility
{

    [System.Serializable]
    public class ProcessInformation
    {
        public string m_processName;
        public int m_processId;
        public string m_processTitle;
        public IntPtr m_intPtrHandle;
        public Process m_source;

        public IntPtrWrapGet GetAsParent()
        {
            return IntPtrTemp.Int(m_processId, true);
        }
        public IntPtrWrapGet GetAsChildren()
        {
            return IntPtrTemp.Int(m_processId, false);
        }
        //public uint m_intPtrHandleRaw;
        //public int m_intPtrHandleRaw2;
    }

    #region USER32


    #region Title

        public static  string GetWindowTitle(IntPtrWrapGet hWn)
        {
            return GetWindowTitle(hWn.GetAsIntPtr());
        }
        public static string GetWindowTitle(IntPtr hWn)
        {   
        object LParam = new object();
        int WParam = 0;
        StringBuilder title = new StringBuilder(1024);
        SendMessage(hWn, 0x000D, WParam, LParam);
        GetWindowText(hWn, title, title.Capacity);
        return title.ToString();

    }
    #endregion Title



    [DllImport("user32.dll", SetLastError = true)]
     static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
    public static bool PostMessage(IntPtrWrapGet hWnd, uint Msg, int wParam, int lParam) { return PostMessage(hWnd.GetAsIntPtr(), Msg, wParam, lParam); }

    [DllImport("user32.dll", SetLastError = true)]
     static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
    public static  bool SendMessage(IntPtrWrapGet hWnd, uint Msg, int wParam, int lParam) { return SendMessage(hWnd.GetAsIntPtr(), Msg, wParam, lParam); }

    [DllImport("user32.dll", SetLastError = true)]
     static extern bool PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
    public static  bool PostMessage(IntPtrWrapGet hWnd, uint Msg, uint wParam, uint lParam) { return PostMessage(hWnd.GetAsIntPtr(), Msg, wParam, lParam); }

    [DllImport("user32.dll", SetLastError = true)]
     static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    public static  bool PostMessage(IntPtrWrapGet hWnd, uint Msg, IntPtr wParam, IntPtr lParam) { return PostMessage(hWnd.GetAsIntPtr(), Msg, wParam, lParam); }

    [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
     static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);
    public static int SendMessage(IntPtrWrapGet hWnd, uint msg, int wParam, IntPtr lParam) {return SendMessage(hWnd.GetAsIntPtr(), msg, wParam, lParam); }



    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    public static void FindWindow(string lpClassName, string lpWindowName, out IntPtrWrapGet process) { process=IntPtrTemp.Int(FindWindow(lpClassName, lpWindowName)); }


    #endregion



    [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);
    [DllImport("User32.dll", EntryPoint = "SendMessage")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, object lParam);

    [DllImport("user32.dll")]
    static extern bool AllowSetForegroundWindow(int dwProcessId);
    public static void AllowSetForegroundWindow(IntPtrWrapGet pointer)
    {
        AllowSetForegroundWindow(pointer.GetAsInt());
    }
    public static void AllowSetForegroundWindow()
    {
        AllowSetForegroundWindow(GetCurrrentUnityAppProcess().GetAsInt());
    }
    public static void AllowProcessCurrentFocus()
    {
        Process p = Process.GetCurrentProcess();
        AllowSetForegroundWindow(p.Id);
        IntPtrWrapGet[] childs = GetProcessIdChildrenWindows(new IntPtrProcessId(p.Id));
        foreach (var item in childs)
            AllowSetForegroundWindow(p.Id);
        AllowSetForegroundWindow((int)p.MainWindowHandle);
        childs = GetProcessIdChildrenWindows(new IntPtrProcessId((int)p.MainWindowHandle));
        foreach (var item in childs)
            AllowSetForegroundWindow(p.Id);
    }

    private static IntPtrWrapGet GetCurrrentUnityAppProcess()
    {
        ProcessesAccessInScene.Instance.GetUnityAppProcess(out IntPtrWrapGet parent, out IntPtrWrapGet firstChildrenDisplay);
        return parent;
    }


    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int xLeftRightTopLeftCorner, int yTopRightTopLeftCorner, int nWidthRight, int nHeightDown, bool bRepaint);
    public static  bool MoveWindow(IntPtrWrapGet hWnd,
        int xLeftRightTopLeftCorner,
        int yTopRightTopLeftCorner,
        int nWidthRight, int nHeightDown,
        bool bRepaint)
    {
        return MoveWindow(hWnd.GetAsIntPtr(), xLeftRightTopLeftCorner, yTopRightTopLeftCorner, nWidthRight, nHeightDown,true);
    }

    public static bool MoveWindow(IntPtrWrapGet hWnd, User32RelativePixelPointLRTB downLeft, User32RelativePixelPointLRTB topRight)
    {

        return MoveWindow(hWnd.GetAsIntPtr(), downLeft.m_pixelLeft2Right, topRight.m_pixelTop2Bot,
            Math.Abs(downLeft.m_pixelLeft2Right - topRight.m_pixelLeft2Right),
            Math.Abs(downLeft.m_pixelTop2Bot - topRight.m_pixelTop2Bot),true);
    }
    public static bool MoveWindowAtCenter(IntPtrWrapGet hWnd, User32RelativePixelPointLRTB center,int width, int height)
    {
        int wh = width / 2;
        int hh = height / 2;
        return MoveWindow(hWnd.GetAsIntPtr(), 
            center.m_pixelLeft2Right - wh,
            center.m_pixelTop2Bot - hh,
            width,
            height,true);
    }
    

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    public static bool ShowWindow(IntPtrWrapGet hWnd, WindowDisplayType windowType) {
        return ShowWindow(hWnd.GetAsIntPtr(), (int)windowType);
    }
   

    public enum WindowDisplayType : int {
        
        Hide=0,
        Normal=1,
        SmallSize=2,
        MaxSize=3,
        Show=5,
        RestoreToDefault=9
        }



    public static void GetParentOfProcessId(IntPtrWrapGet id, out bool found, out IntPtrWrapGet parentId)
    {

        ProcessesAccessInScene.Instance.RefreshIfFirstTime();
        ProcessesAccessInScene.Instance.GetParentOf(id, out found, out parentId);
    }

    public static void GetParentAndChildOf(IntPtrWrapGet pid, out IntPtrWrapGet parent, out IntPtrWrapGet activeChildren)
    {
        if (pid == null || pid.GetAsInt()==0)
        {
            parent = new IntPtrTemp(0, true);
            activeChildren = new IntPtrTemp(0, false);
            return;
        }
        parent = pid;
        WindowIntPtrUtility.FetchFirstChildrenThatHasDimension(pid,
            out bool foundChild, out activeChildren
            );
        if (foundChild) {

            parent = pid;
            return;
        }

            WindowIntPtrUtility.GetParentOfProcessId(pid, out bool foundParent,
                out parent);
        if (!foundParent)
        {
            parent = new IntPtrTemp(pid.GetAsInt(), true);
            activeChildren = new IntPtrTemp(0, false);
            return;
        }
        else
        {
            activeChildren = new IntPtrTemp(pid.GetAsInt(), false);
            return;
        }

    }



    #region CLick With mouse

    public static void MouseDownMiddle()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.MiddleDown);
    }

    public static void MouseDownRight()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.RightDown);
    }

    public static void MouseDownLeft()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.LeftDown);
    }
    public static void MouseUpMiddle()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.MiddleUp);
    }

    public static void MouseUpRight()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.RightUp);
    }

    public static void MouseUpLeft()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.LeftUp);
    }

    public static void MouseClickMiddle()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.MiddleDown);
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.MiddleUp);
    }

    public static void MouseClickRight()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.RightDown);
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.RightUp);
    }

    public static void MouseClickLeft()
    {
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.LeftDown);
        MouseOperations.MouseEventWithCurrentPosition(MouseOperations.MouseEventFlags.LeftUp);
    }

    #endregion

    #region Get Children
    public static IntPtrWrapGet[] GetProcessIdChildrenWindows(IntPtrWrapGet process)
    {
        if (process == null || process.GetAsInt() == 0)
            return new IntPtrWrapGet[0];

        int p =  process.GetAsInt();
        IntPtrWrapGet[] apRet = (new IntPtrWrapGet[256]);
        int iCount = 0;
        IntPtr pLast = IntPtr.Zero;
        do
        {
            pLast = FindWindowEx(IntPtr.Zero, pLast, null, null);
            int iProcess_;
            GetWindowThreadProcessId(pLast, out iProcess_);
            if (iProcess_ == p) apRet[iCount++] = IntPtrTemp.Int( pLast,true);
        } while (pLast != IntPtr.Zero && iCount<256);
        System.Array.Resize(ref apRet, iCount);
        return apRet;
    }
    [DllImport("user32.dll")]
     static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);
    [DllImport("user32.dll")]
     static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);

   
    #endregion

    [DllImport("user32.dll")]
    static extern bool AdjustWindowRectEx(ref RectPadValue lpRect, uint dwStyle,
    bool bMenu, uint dwExStyle);
    [DllImport("User32.dll")]
    public extern static bool GetCursorPos(ref Point pot);
    public struct Point { public int x; public int y; }
    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    public static extern int SetCursorPos(int x, int y);
    public static  int SetCursorPos(in User32CursorPointRelative position) {
        return SetCursorPos(position.m_xLeft2Right, position.m_yTop2Down);
    }
  
    [DllImport("User32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("User32.dll")]
    public static extern bool SetForegroundWindow(int hWnd);
    public static bool SetForegroundWindow(IntPtrWrapGet pointer) {
        //AllowSetForegroundWindow();
        return SetForegroundWindow(pointer.GetAsIntPtr());
    } 


    public static bool IsProcessRunning(string processExactName)
    {
        if(IsProcessRunningByExactName(processExactName))
            return true;
        int id = 0;
        if (int.TryParse(processExactName, out id))
            return IsProcessRunningByParentId(id);
        return false;
    }
    public static bool IsProcessRunningByExactName(string processExactName)
    {
        var p = GetFirstProcessByNameId(processExactName);
        return p != null;
    }
    public static Process GetProcessFromId(int processId)
    {
        return Process.GetProcessById(processId);
    }
    public static Process GetFirstProcessByNameId(string processExactName)
    {
        Process[] processes = Process.GetProcessesByName(processExactName);
        if (processes.Length == 0)
            return null;
        else return processes[0];
    }
    public enum ProcessSearched { Parent, DisplayChildren }
    public static void GetProcessFromIdOrName(string processIdOrName, out bool processFound, out IntPtrWrapGet processRef)
    {
        processFound = false;
        processRef = null;
        Process[] p = Process.GetProcessesByName(processIdOrName);
        if (p.Length > 0)
        {
            processRef = IntPtrProcessId.Int(p[0].Id);
            processFound = true;
            return;
        }


        if (int.TryParse(processIdOrName, out int id))
        {
            if (id == 0)
                return;
            try {
               processRef =  IntPtrProcessId.Int(Process.GetProcessById(id).Id);
               processFound = true;
            } catch (Exception) {}

        }
    }

    public static void GetProcessDisplayChildrenFromIdOrName(string processIdOrName, out bool processFound, out IntPtrWrapGet processRef)
    {
        processFound = false;
        processRef = null;
        Process[] p = Process.GetProcessesByName(processIdOrName);
        if (p.Length > 0)
        {
            FetchFirstChildrenThatHasDimension(IntPtrProcessId.Int(p[0].MainWindowHandle), out processFound, out processRef);
            return;
        }


        if (int.TryParse(processIdOrName, out int id))
        {
            if (id == 0)
                return;
            FetchFirstChildrenThatHasDimension(IntPtrProcessId.Int(id), out processFound, out processRef);
        }
    }




    public static Process[] GetFirstProcessesByNameId(string processExactName)
    {
        return Process.GetProcessesByName(processExactName);
    }
    public static void GetProcessesIndexByNameId(string processExactName,int index, out bool found, out IntPtrWrapGet process)
    {
        process = null;
        GetProcessesIndexByNameId(processExactName,index, out found, out Process p);
        if(found)
            FetchFirstChildrenThatHasDimension(p, out found, out process);
    }
    public static void GetProcessesIndexByNameId(string processExactName, int index, out bool  found, out Process process)
    {
        Process[] p  =  Process.GetProcessesByName(processExactName);
        if (index < p.Length)
        {
            found = true;
            process = p[index];
        }
        else {

            found = false;
            process = null;
        }
    }
    public static bool IsProcessRunningByParentId(int processId)
    {
        Process process;
        try
        {
            process = Process.GetProcessById(processId);
            return process != null && process.Id != 0;

        }
        catch (Exception) { }

        //try
        //{
            GetParentOfProcessId(IntPtrProcessId.Int(processId), out bool found, out IntPtrWrapGet parent);
            process = Process.GetProcessById(parent.GetAsInt());
            return process != null && process.Id != 0;

        //}
        //catch {
        //    return false;
        //}

    }

    public static void RefreshInfoOf(ref DeductedInfoOfWindowSizeWithSource rectInfo)
    {
        FetchWindowInfoUtility.Get(rectInfo.m_pointer, out WindowIntPtrUtility.RectPadValue rect);
        if (rect.IsNotZero())
        {
            rectInfo.SetWith(rect);
        }
    }

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref RectPadValue rectangle);
    public static bool GetWindowRect(IntPtrWrapGet hwnd, ref RectPadValue rectangle)
    {
        return GetWindowRect(hwnd.GetAsIntPtr(), ref rectangle);
    }
    public static bool GetWindowRect(IntPtrWrapGet hwnd, out  IntPtrToRawRect rectangle)
    {
        RectPadValue value = new RectPadValue();
        bool r = GetWindowRect(hwnd,ref   value);
        rectangle = new IntPtrToRawRect(hwnd, value);
        return r;
    }




    [System.Serializable]
    public struct RectPadValue
    {
        //DONT CHANGE THE ORDER Of THE INT left, top, right, bot
        public int m_borderLeft;
        public int m_borderTop;
        public int m_borderRight;
        public int m_borderBottom;

        public bool IsNotZero()
        {
            return m_borderLeft != 0 ||
                 m_borderTop != 0 ||
                 m_borderRight != 0 ||
                 m_borderBottom != 0;
        }
        public bool IsEqualZero()
        {
            return m_borderLeft == 0 &&
                m_borderTop == 0 &&
                m_borderRight == 0 &&
                m_borderBottom == 0;
        }

    }
    [System.Serializable]
    public class IntPtrToRawRect
    {
        public IntPtrWrapGet m_hwnd;
        public int m_intPtr;
        public RectPadValue m_rectInt;

        //public PointToRect(IntPtr hwnd, RectProperty rect)
        //{
        //    this.hwnd = hwnd;
        //    this.m_intPtr = (int)hwnd;
        //    m_rect = rect;
        //    m_rectInt.m_borderLeft = rect.Left;
        //    m_rectInt.m_borderTop = rect.Top;
        //    m_rectInt.m_borderRight = rect.Right;
        //    m_rectInt.m_borderBottom = rect.Bottom;
        //}
        public IntPtrToRawRect(IntPtrWrapGet hwnd, RectPadValue rect)
        {
            this.m_hwnd = hwnd;
            this.m_intPtr = hwnd.GetAsInt();
            m_rectInt = rect;
            //m_rectInt.m_borderLeft = rect.Left;
            //m_rectInt.m_borderTop = rect.Top;
            //m_rectInt.m_borderRight = rect.Right;
            //m_rectInt.m_borderBottom = rect.Bottom;
        }
    }

    //[System.Serializable]
    //public struct RectProperty
    //{
    //    public int Left { get; set; }
    //    public int Top { get; set; }
    //    public int Right { get; set; }
    //    public int Bottom { get; set; }
    //}

    public static void FetchProcessNativeInfoFrom(in string processName ,  out List<ProcessInformation> foundProcesses)
    {
        WindowIntPtrUtility.FetchAllProcesses(out List<ProcessInformation>  processes,null);
        WindowIntPtrUtility.FrechWindowWithExactProcessNameFrom(in processName, in processes, out foundProcesses);
    }

    [ContextMenu("Refresh List")]
    public static void FrechWindowWithExactProcessNameFrom(in string processName, in List<ProcessInformation> processes, out List<ProcessInformation> foundProcesses)
    {
        foundProcesses = new List<ProcessInformation>();
        foreach (ProcessInformation p in processes)
        {
            if (p.m_processName == processName)
                foundProcesses.Add(p);
        }
    }
    [ContextMenu("Refresh List")]
    public static void FrechWindowWithExactProcessName(in string processName, out List<ProcessInformation> processes, out List<ProcessInformation> foundProcesses)
    {
        FetchAllProcesses(out processes,null);
        FrechWindowWithExactProcessNameFrom(in processName, in processes, out foundProcesses);
    }
    [DllImport("kernel32.dll")]
    static extern int GetProcessId(IntPtr handle);
    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    public static void FetchAllProcesses(out List<ProcessInformation> processList, List <string> ignoreName)
    {
        processList = new List<ProcessInformation>();

        Process[] processlist = Process.GetProcesses();
        foreach (Process process in processlist)
        {

            if (ignoreName != null && ignoreName.Contains(process.ProcessName.Trim()))
                continue;


            if (!String.IsNullOrEmpty(process.MainWindowTitle) )
            {
                ProcessInformation win = new ProcessInformation()
                {
                    m_processName = process.ProcessName,
                    m_processId = process.Id,
                    m_processTitle = process.MainWindowTitle,
                    m_intPtrHandle = process.Handle,
                    m_source = process
                };
                //GetWindowThreadProcessId(win.m_intPtrHandle, out win.m_intPtrHandleRaw);
                //win.m_intPtrHandleRaw2 = GetProcessId(win.m_intPtrHandle );

                processList.Add(win);
                //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
            }
        }
    }



    private static Point m_tempPoint = new Point();
    public static void GetCursorPos(out int xPxLeft2Right, out int yPxTop2Bottom)
    {
        GetCursorPos(ref m_tempPoint);
        xPxLeft2Right = m_tempPoint.x;
        yPxTop2Bottom = m_tempPoint.y;
    }

    public static void FetchFirstChildrenThatHasDimension(Process processInfo, out bool foundChild, out IntPtrWrapGet target)
    {
        if (processInfo != null && processInfo.Id != 0)
        {
            FetchFirstChildrenThatHasDimension(IntPtrProcessId.Int(processInfo.Id), out foundChild, out target);
        }
        else { 
            foundChild = false;
            target = null;
        }
    }
    public static void FetchFirstChildrenThatHasDimension(int processInfoId, out bool foundChild, out IntPtrWrapGet target)
    {
        
        FetchFirstChildrenThatHasDimension(IntPtrProcessId.Int(processInfoId), out foundChild, out target);
        
    }
    public static void FetchFirstChildrenThatHasDimension(IntPtrWrapGet intPtr, out bool foundChild, out IntPtrWrapGet target)
    {
        RectPadValue rect = new RectPadValue();
        IntPtrWrapGet[] ptrs = GetProcessIdChildrenWindows(intPtr);
        for (int i = 0; i < ptrs.Length; i++)
        {
            GetWindowRect(ptrs[i], ref rect);
            if (rect.IsNotZero())
            {
                foundChild = true;
                target = ptrs[i] ;
                return;
            }
        }
        foundChild = false;
        target = null;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    public static void GetCurrentChildrenFocusProcessId(out IntPtrWrapGet m_processId)
    {
        m_processId = IntPtrTemp.Int( GetForegroundWindow(),false );
    }
    public static void GetCurrentProcessId(out IntPtrWrapGet processId)
    {
        processId = IntPtrTemp.Int(GetForegroundWindow(), false);
    }
    public static IntPtrWrapGet GetCurrentProcessId()
    {
        return IntPtrTemp.Int(GetForegroundWindow(), false);
    }

    public static void GetCurrentProcessId(out IntPtrWrapGet mainHandle, out IntPtrWrapGet processId)
    {
        processId = IntPtrTemp.Int( Process.GetCurrentProcess().Id);
        mainHandle = IntPtrTemp.Int(Process.GetCurrentProcess().MainWindowHandle);


    }

}