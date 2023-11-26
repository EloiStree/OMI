//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Eloi;
//using System.IO;
//using System;
//using System.Diagnostics;

//public class LaunchHiddenExecutableWhenInAndOutMono : MonoBehaviour
//{
//    public AbstractMetaAbsolutePathDirectoryMono m_startFolderHidden;
//    public AbstractMetaAbsolutePathDirectoryMono m_quitFolderHidden;
//    public bool m_createIfMissing=true;
//    void Start()
//    {
//        string p = m_startFolderHidden.GetPath();
//        if (m_createIfMissing && !Directory.Exists(p))
//            Directory.CreateDirectory(p);
//        string[] paths= Directory.GetFiles(p, "*", SearchOption.AllDirectories);
//        foreach (var path in paths)
//        {
//            string file = Path.GetFileName(path);
//            if(!file.StartsWith(m_ignoreFileIfStartWith))
//                LaunchHiddenFileFromPath(path);
//        }
//    }

//    void OnApplicationQuit()
//    {
//        string p = m_quitFolderHidden.GetPath();
//        if (m_createIfMissing && !Directory.Exists(p))
//            Directory.CreateDirectory(p);

//        string[] paths = Directory.GetFiles(p, "*", SearchOption.AllDirectories); 
//        foreach (var path in paths)
//        {
//            string file = Path.GetFileName(path);
//            if (!file.StartsWith(m_ignoreFileIfStartWith))
//                LaunchHiddenFileFromPath(path);
//        }
//    }

//    public void LaunchHiddenFileFromPath(string path)
//    {
//        ProcessStartInfo psi = new ProcessStartInfo();
//        psi.FileName = path;
//        psi.UseShellExecute = false; // This is essential for CreateNoWindow to work
//        psi.CreateNoWindow = true;   // Hide the process window
//        psi.WindowStyle = ProcessWindowStyle.Hidden; // Hide the process window

//        Process process = new Process();
//        process.StartInfo = psi;

//        try
//        {
//            process.Start();
//        }
//        catch (Exception ex)
//        {
//            UnityEngine. Debug.Log("Error launching hidden file: " + ex.Message);
//        }
//    }
//}
