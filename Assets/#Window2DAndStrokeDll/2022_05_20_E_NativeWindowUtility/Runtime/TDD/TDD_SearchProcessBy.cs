using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_SearchProcessBy : MonoBehaviour
{




    void Start()
    {
        ProcessesAccessInScene.Instance.RefreshListOfProcesses();

       // ProcessSearchUtility.GetParentWithChildrensOf
    }

   

}


[System.Serializable]
public struct ProcessInfoHolder
{
    public string m_processName;
    public string m_processTitle;
    public IntPtrProcessId m_parent;
    public IntPtrProcessId m_children;

}
[System.Serializable]

public struct ProcessTarget_ByUnkownId
{
    public int m_unkownId;
}
[System.Serializable]

public struct ProcessTarget_ByChil
{
    public int m_unkownId;
}
[System.Serializable]

public struct ProcessTarget_ByExactName
{
    public string m_exactName;
}
[System.Serializable]

public struct ProcessTarget_ByRegexOnName
{
    public string m_exactName;
}
[System.Serializable]

public struct ProcessTarget_ByContainText
{
    public string m_exactName;
}