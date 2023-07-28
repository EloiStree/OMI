using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static WindowIntPtrUtility;

public class ProcessesAccessInScene : Eloi.GenericSingletonOfMono<ProcessesAccessMono> { }




public interface IProcessMainInfo : IProcessMainInfoGet, IProcessMainInfoSet
{
}
public interface IProcessMainInfoGet
{

    public void GetProcessExactName(out string name);
    public void GetProcessTitle(out string name);
    public void GetProcessParent(out IntPtrWrapGet parent);
    public void GetProcessChildrens(out IntPtrWrapGet[] childrens);
    public void WasDisplayChildrenComputed(out bool isDisplayChildrenComputed);
    public void GetDisplayChildrenComputed(out IntPtrWrapGet children);
    public void GetLinkedProcess(out Process linkedProject);
    public void GetDateTimeWhenAppearInRefresh(out DateTime whenProcess);

}
public interface IProcessMainInfoSet
{

    public void SetProcessName( string name);
    public void SetProcessTitle( string name);
    public void SetProcessParent( IntPtrWrapGet parent);
    public void SetProcessChildrens( IntPtrWrapGet[] childrens);
    public void SetDisplayChildrenComputed( IntPtrWrapGet children);
    public void SetLinkedProcess( Process linkedProject);

}
[System.Serializable]
public class ProcessMainInfo : IProcessMainInfo
{
    public string m_processName;
    public string m_processTitle;
    public IntPtrProcessId m_parent;
    public List<IntPtrProcessId> m_childrens = new List<IntPtrProcessId>();
    public bool m_displayChildrenComputed;
    public IntPtrProcessId m_displayChildren;
    public DateTime m_dateWhenAppearInRefresh;
    public Process m_linkedProcess;

    public void AddChildren(IntPtrWrapGet child) {
        RemoveChildren(child);
        m_childrens.Add(new IntPtrProcessId(child.GetAsInt()));

    }
    public void RemoveChildren(IntPtrWrapGet child) {

        int id = child.GetAsInt();
        m_childrens = m_childrens.Where(k => k.GetAsInt() != id).ToList();
    }

    public void GetDateTimeWhenAppearInRefresh(out DateTime whenProcess) => whenProcess = m_dateWhenAppearInRefresh;
    public void GetDisplayChildrenComputed(out IntPtrWrapGet children) => children = (IntPtrWrapGet)m_displayChildren;

   
    public void GetLinkedProcess(out Process linkedProject) => linkedProject = m_linkedProcess;

    public void GetProcessChildrens(out IntPtrWrapGet[] childrens) => childrens = m_childrens.
        Select(k => (IntPtrWrapGet)k).ToArray();
    public void GetProcessExactName(out string exactName) => exactName = m_processName;

    public void GetProcessParent(out IntPtrWrapGet parent) => parent = m_parent;

    public void GetProcessTitle(out string title) => title = m_processTitle;

   
    public void SetDisplayChildrenComputed(IntPtrWrapGet children)
    {
        m_displayChildrenComputed = true;
        if (children == null)
            m_displayChildren = IntPtrProcessId.Zero;
        else 
            m_displayChildren.Set(children.GetAsInt());
    }


    public void SetLinkedProcess(Process linkedProject)
    {
        m_linkedProcess = linkedProject;
    }

    public void SetProcessChildrens(IntPtrWrapGet[] childrens)
    {
        m_childrens = childrens.Select(k => new IntPtrProcessId(k.GetAsInt())).ToList();
    }

    public void SetProcessName(string name)
    {
        m_processName = name;
    }

    public void SetProcessParent(IntPtrWrapGet parent)
    {
        m_parent = new IntPtrProcessId(parent.GetAsInt());
    }

    public void SetProcessTitle(string processTitle)
    {
        m_processTitle = processTitle;
    }

    public void WasDisplayChildrenComputed(out bool isDisplayChildrenComputed)
    {
        isDisplayChildrenComputed = m_displayChildrenComputed;
    }
    public void SetAsFirstAppearedTo(DateTime date) {
        m_dateWhenAppearInRefresh = date;
    }

    public bool HasDisplayChildren()
    {
        return m_displayChildrenComputed && m_displayChildren.GetAsInt() != 0;
    }
}



public class ProcessesAccessMono : MonoBehaviour
{

    public TextAsset[] m_processNameToIgnoreFile; 
    public List<string> m_ignoreByExactName= new List<string>();
    public void ComputeChildrenFor(IProcessMainInfo targetToRefresh)
    {
        targetToRefresh.GetProcessParent(out IntPtrWrapGet parent);
        WindowIntPtrUtility.FetchFirstChildrenThatHasDimension(parent, out bool has, out IntPtrWrapGet child);
        targetToRefresh.SetDisplayChildrenComputed(child);
    }
    public void RefreshInfoOfProcessesByExactName(IProcessMainInfo targetToRefresh)
    {

    }
    public void RefreshInfoOfProcessesByExactName(string exactName)
    {
        Process[] p = Process.GetProcessesByName(exactName);
        for (int i = 0; i < p.Length; i++)
        {
            RefreshSoloProcessValue(p[i]);
            RefreshGroupProcessValue(i,p[i]);
        }
    }

    public void RefreshSoloProcessValue(Process p)
    {
       // if(m_allProcessInfo.ContainsKey(p.))
    }
    private void RefreshGroupProcessValue(int indnex, Process p)
    {

    }


    public Dictionary<int, ProcessStartInfo> m_allProcessInfo = new Dictionary<int, ProcessStartInfo>();

    public List<ProcessStartInfo> m_allProcessInfoDebug = new List<ProcessStartInfo>();


     GroupOfProcessesInfo m_lastRefreshOfProcess = new GroupOfProcessesInfo();
  
    public Dictionary<int, ProcessMainInfo> m_processMainInfo = new Dictionary<int, ProcessMainInfo>();
    public List<ProcessMainInfo> m_processMainInfoDebug = new List<ProcessMainInfo>();

    public RelayFoundInfo m_onRefresh= new RelayFoundInfo();
    [System.Serializable]
    public class RelayFoundInfo : UnityEvent<GroupOfProcessesInfo> { }

    public bool m_autoLoadAllAtWake=true;

    public IntPtrWrapGet m_current;
    public IntPtrWrapGet m_parent;
    public IntPtrWrapGet m_firstDisplayChild;
    public int m_currentDebug;
    public int m_parentDebug;
    public int m_firstDisplayChildDebug;




    public void FetchProcessInfoBasedOnIndex(in string processIdName, in int processIndex, out bool found, out TargetParentProcessIntPtr pointer)
    {
        found = false;
        pointer = new TargetParentProcessIntPtr();
        m_lastRefreshOfProcess.GetWith(out List<WindowIntPtrUtility.ProcessInformation> processes);
        int count=0;
        foreach (var item in processes)
        {
            if (Eloi.E_StringUtility.AreEquals(item.m_processName, processIdName,true, true)) {
                if (processIndex == count) {
                    found = true;
                    pointer.SetAsInt(item.m_processId);
                    return;
                }
                count++;
            }
        }
    }

    public void GetParentOf(IntPtrWrapGet id, out bool found, out IntPtrWrapGet parentId)
    {
        int index = id.GetAsInt();
        found = m_processMainInfo.ContainsKey(index);
        if (found)
        {
            ProcessMainInfo info = m_processMainInfo[index];
            info.GetProcessParent(out parentId);
        }
        else parentId = null;
    }
    public void GetChildrenDisplayOf(IntPtrWrapGet id, out bool found, out IntPtrWrapGet childrenOf)
    {
        int index = id.GetAsInt();
        found = m_processMainInfo.ContainsKey(index);
        if (found)
        {
            ProcessMainInfo info = m_processMainInfo[index];
            info.GetDisplayChildrenComputed(out childrenOf);
        }
        else childrenOf = null;
    }

    public void RefreshIfFirstTime()
    {
        if (!m_wasRefreshOnce)
            RefreshListOfProcesses();
    }

    void Awake()
    {
        if (m_autoLoadAllAtWake)
            RefreshListOfProcesses();
        WindowIntPtrUtility.AllowProcessCurrentFocus();

    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            RefreshListOfProcesses();
    }
    private void OnApplicationPause(bool isPause)
    {
        if (!isPause)
            RefreshListOfProcesses();
    }
    ChildAndParentTree m_childParent = new ChildAndParentTree();
    bool m_wasRefreshOnce;
    [ContextMenu("Refresh")]
    public void RefreshListOfProcesses()
    {
        m_wasRefreshOnce = true;
        List<string> ignore = GetListOfExactNameToIgnore();
        WindowIntPtrUtility.FetchAllProcesses(out
            List<WindowIntPtrUtility.ProcessInformation> allProcess, ignore);
        m_lastRefreshOfProcess.SetWith(allProcess);

        m_processMainInfo.Clear();
        m_childParent.ClearAll();
        for (int i = 0; i < allProcess.Count; i++)
        {
            int id = allProcess[i].m_processId;
            IntPtrWrapGet[] ps = WindowIntPtrUtility.GetProcessIdChildrenWindows(IntPtrTemp.Int(id));
            m_childParent.AddPair(id, ps.Where(k => k != null).Select(k => k.GetAsInt()).ToArray());

    
        }
        ComputeFullInfo();
        m_onRefresh.Invoke(m_lastRefreshOfProcess);
    }

    private List<string> GetListOfExactNameToIgnore()
    {
        List<string> ignore = new List<string>();
        ignore.AddRange(m_ignoreByExactName);
        for (int i = 0; i < m_processNameToIgnoreFile.Length; i++)
        {
            foreach (var t in m_processNameToIgnoreFile[i].text.Split("\n"))
            {
                ignore.Add(t.Trim()); 
            }
            


        }
        ignore = ignore.Distinct().ToList();
        return ignore;
    }

    private void ComputeFullInfo()
    {
       // m_processMainInfo.Clear();
        m_lastRefreshOfProcess.GetWith(out List<WindowIntPtrUtility.ProcessInformation> process );
        foreach (var item in process)
        {
            PushOrUpdateProcessInfo(item);
        }
        m_processMainInfoDebug = m_processMainInfo.Values.ToList();
    }
    private void PushOrUpdateProcessInfo(Process item)
    {
        if (!String.IsNullOrEmpty(item.MainWindowTitle))
        {
            ProcessInformation win = new ProcessInformation()
            {
                m_processName = item.ProcessName,
                m_processId = item.Id,
                m_processTitle = item.MainWindowTitle,
                m_intPtrHandle = item.Handle,
                m_source = item
            };
            PushOrUpdateProcessInfo(win);
        }

    }
        private void PushOrUpdateProcessInfo(WindowIntPtrUtility.ProcessInformation item)
    {
        m_childParent.GetTopParentOf(item.m_processId, out bool found, out int parent);
        ProcessMainInfo info = null;
        if (!m_processMainInfo.ContainsKey(parent))
        {
            info = new ProcessMainInfo();
            info.SetProcessParent(IntPtrProcessId.Int(parent));
            info.SetAsFirstAppearedTo(DateTime.Now);
            m_processMainInfo.Add(parent, info);
        }
        else info = m_processMainInfo[parent];

      

        info.SetProcessName(item.m_processName);
        info.SetLinkedProcess(item.m_source);
        info.SetProcessTitle(item.m_processTitle);
        WindowIntPtrUtility.FetchFirstChildrenThatHasDimension(parent, out bool hasChild, out IntPtrWrapGet child);
        info.SetDisplayChildrenComputed(child);

        m_childParent.GetChildrenOf(item.m_processId, out List<int> directchilds);
        foreach (var i in directchilds)
        {
            info.AddChildren(IntPtrProcessId.Int(i));
        }

        if (info.HasDisplayChildren()) {
            info.GetDisplayChildrenComputed(out IntPtrWrapGet display);
            //if(display != null) 
            //WindowIntPtrUtility.GetWindowTitle(display);

        }
    }



    //public void FetchListOfProcessesBasedOnName(string processName,
    //    out GroupOfProcessesParentToChildrens found,
    //    bool reloadProcesses) {

    //    if (reloadProcesses)
    //    {
    //        FetchAndUpdateCurrentProcess();
    //    }

    //    found = new GroupOfProcessesParentToChildrens();
    //    m_lastRefreshOfProcess.GetWith(out List<WindowIntPtrUtility.ProcessInformation> info);
    //    for (int i = 0; i < info.Count; i++)
    //    {
    //        if (Eloi.E_StringUtility.AreEquals(processName, info[i].m_processName, true, true))
    //        {
    //            found.AddFromId(info[i].m_processId, info[i].m_processName);
    //        }
    //    }
    //}

    private void FetchAndUpdateCurrentProcess()
    {
        WindowIntPtrUtility.FetchAllProcesses(out
       List<WindowIntPtrUtility.ProcessInformation> allProcess, m_ignoreByExactName);
        m_lastRefreshOfProcess.SetWith(allProcess);
    }

    public void GetUnityAppProcess(out IntPtrWrapGet parent, out IntPtrWrapGet firstChildrenDisplay)
    {
        parent = m_parent;
        firstChildrenDisplay = m_firstDisplayChild;
    }

    public void GetProcessesBasedOnName(string processNameToFetch, out List<IProcessMainInfoGet> process, bool trim = true, bool ignoreCase = false)
    {
        process = new List<IProcessMainInfoGet>();
        string name = "";
        foreach (var item in m_processMainInfoDebug)
        {
            item.GetProcessExactName(out name);
            Eloi.E_StringUtility.AreEquals(processNameToFetch, name, ignoreCase,trim); 
            process.Add(item);
        }
    }
}

[System.Serializable]
public class GroupOfProcessesInfo {
    [SerializeField]  List<WindowIntPtrUtility.ProcessInformation>
        m_listOfProcesses = new List<WindowIntPtrUtility.ProcessInformation>();

    public void SetWith(List<WindowIntPtrUtility.ProcessInformation> info) {
        m_listOfProcesses = info;
    }
    public void GetWith(out List<WindowIntPtrUtility.ProcessInformation> info) {
        info = m_listOfProcesses;
    }

}

//[System.Serializable]
//public class GroupOfProcessesPairParentToChildren
//{
//    public List<ProcessIdChildrenPair> m_processesAndChildrens = new List<ProcessIdChildrenPair>();

//    public void Clear() { m_processesAndChildrens.Clear(); }
//    public void AddProcess(ProcessIdChildrenPair process)
//    {
//        m_processesAndChildrens.Add(process);
//    }
//    public void AddFromId(int id)
//    {

//        IntPtrWrapGet[] ps = WindowIntPtrUtility.GetProcessIdChildrenWindows(IntPtrTemp.Int(  id));
//        for (int i = 0; i < ps.Length; i++)
//        {
//            AddProcess(new ProcessIdChildrenPair(id, ps[i].GetAsInt()));
//        }
//    }
//}
//[System.Serializable]
//public class GroupOfProcessesParentToChildrens
//{
//    public List<ProcessIdWithChildGroupInfo> m_processesAndChildrens = new List<ProcessIdWithChildGroupInfo>();

//    public void Clear() { m_processesAndChildrens.Clear(); }
//    public void AddProcess(ProcessIdWithChildGroupInfo process)
//    {
//        m_processesAndChildrens.Add(process);
//    }
//    public void AddFromId(int id, string debugName="")
//    {

//        IntPtrWrapGet[] ps = WindowIntPtrUtility.GetProcessIdChildrenWindows(IntPtrTemp.Int( id));
//        foreach (var item in ps)
//        {
//        ProcessIdWithChildGroupInfo i = new ProcessIdWithChildGroupInfo(id, item.GetAsInt());
//        i.SetDebugName(debugName);
//        AddProcess(i);

//        }
//    }
//}


public class ChildAndParentTree {

    public Dictionary<int, IdToChildrens> m_allId = new Dictionary<int, IdToChildrens>();

    public void AddPair(int id, params int [] childs) {

        if (!m_allId.ContainsKey(id))
            m_allId.Add(id, new IdToChildrens(id));
        foreach (var item in childs)
        {
            m_allId[id].AddChild(item);
        }
    }

    public void ClearAll() => m_allId.Clear();
    public void ClearOne(int id) {
        if (m_allId.ContainsKey(id))
            m_allId[id].m_children.Clear();
    }

    public void GetChildrenOf(int id, out List<int> childrens) {
        if (m_allId.ContainsKey(id))
            childrens = m_allId[id].m_children;
        else childrens = null;
    }

    public bool IsRelativeParent(int id) => m_allId.ContainsKey(id);
    public void GetParentOf(int id, out bool foundParent, out int parent)
    {

        foreach (var item in m_allId.Values)
        {
            if (item.Contains(id))
            {
                foundParent = true;
                parent = item.m_id;
            }
        }
        foundParent = false;
        parent = -1;
    }

    public void GetTopParentOf(int id, out bool foundParent, out int parent)
    {
        bool reachTop = false;
        foundParent = false;
        int nextToSearch = id;
        while (!reachTop) {
            GetParentOf(nextToSearch, out bool loopFound, out  nextToSearch);
            if (loopFound)
                foundParent = true;
            if (!loopFound)
                reachTop = true;
        }
        if (foundParent)
            parent = nextToSearch;
        else
            parent = id;
    }

}


public class IdToChildrens {
    public int m_id;
    public List<int> m_children;

    public IdToChildrens(int id)
    {
        m_id = id;
        m_children = new List<int>();
    }
    public void AddChild(int idChild)
    {
        while (m_children.Contains(idChild))
            m_children.Remove(idChild);
        m_children.Add(idChild);
    }
    public void RemoveChild(int idChild)
    {
        while (m_children.Contains(idChild))
            m_children.Remove(idChild);
    }

    public bool Contains(int id) {
        return m_children.Contains(id);
    }
}
