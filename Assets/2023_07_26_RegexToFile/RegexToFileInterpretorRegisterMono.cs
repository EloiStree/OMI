using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RegexToFileInterpretorRegisterMono : MonoBehaviour
{
    public RegexToDirectoryInterpreterRegister m_directoryInterpreter;
    public RegexToDoubleFileInterpreterRegister m_doubleInterpreter;

    public void Clear()
    {
        m_directoryInterpreter.Clear();
        m_doubleInterpreter.Clear();
    }
}



[System.Serializable]
public class RegexToDoubleFileInterpreterRegister
{

    public List<RegexToDoubleDateFileInterpreterContainer> m_userIntent = new List<RegexToDoubleDateFileInterpreterContainer>();

    public void GetInterpreterFromText(string text, out bool foundOne, out RegexToDoubleDateFileInterpreterContainer regexInterpreter)
    {
        for (int i = 0; i < m_userIntent.Count; i++)
        {
            if (m_userIntent[i] != null &&
                m_userIntent[i].m_regexToLookFor != null &&
                m_userIntent[i].m_regexToLookFor.Length > 0)
            {
                if (Regex.IsMatch(text, m_userIntent[i].m_regexToLookFor))
                {
                    foundOne = true;
                    regexInterpreter = m_userIntent[i];
                    return;
                }
            }
        }
        foundOne = false;
        regexInterpreter = null;
    }

    public void Clear()
    {
        m_userIntent.Clear();
    }

    public void Add(RegexToDoubleDateFileInterpreterContainer interpretor)
    {
        m_userIntent.Add(interpretor);
    }
    public void Add(IEnumerable<RegexToDoubleDateFileInterpreterContainer> interpretor)
    {
        foreach (var item in interpretor)
        {
            m_userIntent.Add(item);
        }
    }
    public void Add(params RegexToDoubleDateFileInterpreterContainer[] interpretor)
    {
        foreach (var item in interpretor)
        {
            m_userIntent.Add(item);
        }
    }
}




[System.Serializable]
public class RegexToDirectoryInterpreterRegister
{

    public List<RegexToCommandAsNewFileInterpreterContainer> m_userIntent = new List<RegexToCommandAsNewFileInterpreterContainer>();

    public void GetInterpreterFromText(string text, out bool foundOne, out RegexToCommandAsNewFileInterpreterContainer regexInterpreter)
    {
        for (int i = 0; i < m_userIntent.Count; i++)
        {
            if (m_userIntent[i] != null &&
                m_userIntent[i].m_regexToLookFor != null &&
                m_userIntent[i].m_regexToLookFor.Length > 0)
            {
                if (Regex.IsMatch(text, m_userIntent[i].m_regexToLookFor))
                {
                    foundOne = true;
                    regexInterpreter = m_userIntent[i];
                    return;
                }
            }
        }
        foundOne = false;
        regexInterpreter = null;
    }


    public void Clear()
    {
        m_userIntent.Clear();
    }

    
    public void Add(RegexToCommandAsNewFileInterpreterContainer interpretor)
    {
        m_userIntent.Add(interpretor);
    }
    public void Add(IEnumerable<RegexToCommandAsNewFileInterpreterContainer> interpretor)
    {
        foreach (var item in interpretor)
        {
            m_userIntent.Add(item);
        }
    }
    public void Add(params RegexToCommandAsNewFileInterpreterContainer[] interpretor)
    {
        foreach (var item in interpretor)
        {
            m_userIntent.Add(item);
        }
    }
}
