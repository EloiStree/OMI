using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class TileLine
{

    public string m_rawReceived;
    public string[] m_tokens;
    public TileLine(string line)
    {
        m_rawReceived = line;
        m_tokens = line.Split('♦');
    }
    public int GetCount() { return m_tokens.Length; }
    public string GetValue(int index)
    {
        return m_tokens[index];
    }

    public List<string> GetAsList()
    {
        return m_tokens.ToList();
    }
}