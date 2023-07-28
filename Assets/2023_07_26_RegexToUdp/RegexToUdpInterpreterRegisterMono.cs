using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RegexToUdpInterpreterRegisterMono : MonoBehaviour
{
    public RegexToUdpInterpreterRegister m_register;


    public void GetInterpreterFromText(string text, out  bool foundOne, out RegexToUdpInterpreterContainer regexInterpreter) {
        m_register. GetInterpreterFromText(text, out foundOne, out regexInterpreter);
    }

    public void Clear() {
        m_register.Clear();
    }

    public void Add(string regexToLookFor, string ipAndPortToTarget)
    {
        m_register.Add(regexToLookFor, ipAndPortToTarget);
    }
    public void Add(RegexToUdpInterpreterContainer interpretor)
    {
        m_register.Add(interpretor);
    }
    public void Add(IEnumerable<RegexToUdpInterpreterContainer> interpretor)
    {

        m_register.Add(interpretor);
    }
    public void Add(params RegexToUdpInterpreterContainer [] interpretor)
    {
        m_register.Add(interpretor);
    }    
}

[System.Serializable]
public class RegexToUdpInterpreterRegister
{

    public List<RegexToUdpInterpreterContainer> m_regexToUdp = new List<RegexToUdpInterpreterContainer>();

    public void GetInterpreterFromText(string text, out bool foundOne, out RegexToUdpInterpreterContainer regexInterpreter)
    {
        for (int i = 0; i < m_regexToUdp.Count; i++)
        {
            if (m_regexToUdp[i] != null &&
                m_regexToUdp[i].m_regexToLookFor != null &&
                m_regexToUdp[i].m_regexToLookFor.Length>0 ) {
                if (Regex.IsMatch(text, m_regexToUdp[i].m_regexToLookFor)) {
                    foundOne = true;
                    regexInterpreter = m_regexToUdp[i];
                    return;
                }
            }
        }
        foundOne = false;
        regexInterpreter = null;
    }


    public void Clear()
    {
        m_regexToUdp.Clear();
    }

    public void Add(string regexToLookFor, string ipAndPortToTarget)
    {
        m_regexToUdp.Add(new RegexToUdpInterpreterContainer(regexToLookFor, ipAndPortToTarget));
    }
    public void Add(RegexToUdpInterpreterContainer interpretor)
    {
        m_regexToUdp.Add(interpretor);
    }
    public void Add(IEnumerable<RegexToUdpInterpreterContainer> interpretor)
    {
        foreach (var item in interpretor)
        {
            m_regexToUdp.Add(item);
        }
    }
    public void Add(params RegexToUdpInterpreterContainer[] interpretor)
    {
        foreach (var item in interpretor)
        {
            m_regexToUdp.Add(item);
        }
    }
}
