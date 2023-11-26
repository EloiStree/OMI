using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class WindowFileLauncherMono : MonoBehaviour
{


    public string m_ignoreFileIfStartWith = "_";
    public void LaunchTargetFileFromPathAsHidden(string pathToLaunch)
    {
        LaunchTargetFileFromPath( true , pathToLaunch );
    }
    public void LaunchTargetFileFromPathAsVisible(string pathToLaunch)
    {
        LaunchTargetFileFromPath( false , pathToLaunch );
    }
    [DllImport("user32.dll")]
    static extern int SetWindowText(IntPtr hWnd, string text);


    public UnityProcessEvent m_processLaunched;
    [System.Serializable]
    public class UnityProcessEvent : UnityEvent<Process> { }


    public void LaunchTargetFileFromPath(bool launchAsHidden, string pathToLaunch) {

        if (m_ignoreFileIfStartWith.Length > 0 && pathToLaunch.StartsWith(m_ignoreFileIfStartWith))
            return;

        string fileName = Path.GetFileName(pathToLaunch);
        string forwardExe = "";
        if (fileName.ToLower().EndsWith(".py"))
            forwardExe = "python3 ";
        if (fileName.ToLower().EndsWith(".jar"))
            forwardExe = "java -jar ";

        if (forwardExe.Length > 0)
        {

            string anyCommand= "Title "+fileName+" \n\r" + forwardExe +" \""+ pathToLaunch+ "\"";
            //var proc1 = new ProcessStartInfo();
            //UnityEngine.Debug.Log(anyCommand);
            ////proc1.UseShellExecute = true;
            ////proc1.WorkingDirectory = @"C:\Windows\System32";
            //proc1.FileName = @"C:\Windows\System32\cmd.exe";
            //proc1.Verb = "runas";
            //proc1.Arguments = "/c " + anyCommand;
            //proc1.WindowStyle = launchAsHidden? ProcessWindowStyle.Hidden: ProcessWindowStyle.Normal;
            //Process.Start(proc1);

            Eloi.E_StringByte64Utility.GetText64FromText(pathToLaunch, out string b64);
            pathToLaunch = Application.dataPath + "/" + b64 + ".bat";
            File.WriteAllText(pathToLaunch, anyCommand);
        }


            Process process=null;

            if (launchAsHidden)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = pathToLaunch;
                psi.UseShellExecute = false; 
                psi.CreateNoWindow = true;   
                psi.WindowStyle = ProcessWindowStyle.Hidden; 

                process = new Process();
                process.StartInfo = psi;

                try
                {
                    process.Start();
                    m_processLaunched.Invoke(process);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("Error launching hidden file: " + ex.Message);
                }
            }
            else {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = pathToLaunch;
                process = new Process();
                process.StartInfo = psi;

                try
                {
                    process.Start();
                     m_processLaunched.Invoke(process);


                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("Error launching  file: " + ex.Message);
                }

            m_setTitleDelayQueue.Add(new DelaySetTitle()
            {
                m_timeLeftInSeconds = 10f,
                m_process = process,
                m_title = fileName
            });

            m_processTitleToKill.Add(fileName);
        }

    }

    public List<string> m_processTitleToKill = new List<string>();


    private void OnDestroy()
    {
        KillAllProcessByTitleLaunched();
    }
    private void OnApplicationQuit()
    {
        KillAllProcessByTitleLaunched();
    }

    public void KillAllProcessByTitleLaunched()
    {

//        string killCommand = "@echo off"
//+ "\nsetlocal enabledelayedexpansion"
//+ "\n"
//+ "\nREM List of process titles to be killed"
//+ "\nset \"ProcessTitles ="+string.Join(" ", m_processTitleToKill) +"\""
//+ "\n"
//+ "\nREM Iterate through each process title and kill the corresponding processes"
//+ "\nfor %% i in (% ProcessTitles %) do ("
//+ "\n    for / f \"tokens =2 delims=,\" %% a in ('tasklist /v /fo csv ^| findstr /i \" %%i\"') do ("
//+ "\n          set \"PID =%%~a\""
//+ "\n        taskkill / pid!PID! / f"
//+ "\n        echo Process with title \" %%i\" and PID !PID! has been terminated."
//+ "\n    )"
//+ "\n)"
//+ "\nendlocal" +
//"\npause";

//        UnityEngine.Debug.Log(killCommand);

//        string p  = Application.dataPath + "/KillTitle.bat";
//        File.WriteAllText(p, killCommand);

//        ProcessStartInfo psi = new ProcessStartInfo();
//        psi.FileName = p;
//        Process process = new Process();
//        process.StartInfo = psi;

//        try
//        {
//            process.Start();


//        }
//        catch (Exception ex)
//        {
//            UnityEngine.Debug.Log("Error launching  file: " + ex.Message);
//        }

    }


    private void Update()
    {
        for (int i = m_setTitleDelayQueue.Count-1; i >=0; i--)
        {
            m_setTitleDelayQueue[i].m_timeLeftInSeconds -= Time.deltaTime;
            if (m_setTitleDelayQueue[i].m_timeLeftInSeconds < 0f) { 
                if(m_setTitleDelayQueue[i].m_process != null)
                   SetWindowText(m_setTitleDelayQueue[i].m_process.MainWindowHandle, m_setTitleDelayQueue[i].m_title) ;
                m_setTitleDelayQueue.RemoveAt(i);
            }
        }
    }

    public List<DelaySetTitle> m_setTitleDelayQueue = new List<DelaySetTitle>();
    [System.Serializable]
    public class DelaySetTitle {
        public float m_timeLeftInSeconds;
        public Process m_process;
        public string m_title;
    }
}
