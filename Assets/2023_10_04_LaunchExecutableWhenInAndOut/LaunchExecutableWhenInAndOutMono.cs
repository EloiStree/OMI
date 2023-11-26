using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;
using System.IO;

public class LaunchExecutableWhenInAndOutMono : MonoBehaviour
{
    public AbstractMetaAbsolutePathDirectoryMono m_startFolder;
    public AbstractMetaAbsolutePathDirectoryMono m_quitFolder;
    public AbstractMetaAbsolutePathDirectoryMono m_startFolderHidden;
    public AbstractMetaAbsolutePathDirectoryMono m_quitFolderHidden;
    public bool m_createIfMissing=true;
    public UnityStringEvent m_pathToExecute;
    public UnityStringEvent m_pathToExecuteAsHidden;
    void Start()
    {
        PushPathVisibile(m_startFolder, false);
        PushPathVisibile(m_startFolderHidden, true);
    }


    void OnApplicationQuit()
    {
        PushPathVisibile(m_quitFolder, false);
        PushPathVisibile(m_quitFolderHidden, true);
    }


    private void PushPathVisibile(AbstractMetaAbsolutePathDirectoryMono target, bool hidden)
    {
        string p = target.GetPath();
        if (m_createIfMissing && !Directory.Exists(p))
            Directory.CreateDirectory(p);
        string[] paths = Directory.GetFiles(p, "*", SearchOption.AllDirectories);
        foreach (var path in paths)
        {
            if (hidden)
                m_pathToExecuteAsHidden.Invoke(path);
            else
                m_pathToExecute.Invoke(path);
        }
    }
}
