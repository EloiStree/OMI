using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RegexToUdpInterpreterContainer 
{
    public string m_regexToLookFor;
    public string m_ipv4AndPortToTarget;
    public ExportType m_exportType = ExportType.Undefined;
    public enum ExportType { UTF8, Unicode, Undefined}

    public RegexToUdpInterpreterContainer()
    {}

    public RegexToUdpInterpreterContainer(string regexToLookFor, string ipv4AndPortToTarget)
    {
        m_regexToLookFor = regexToLookFor;
        m_ipv4AndPortToTarget = ipv4AndPortToTarget;
        m_exportType = ExportType.Undefined;
    }
    public RegexToUdpInterpreterContainer(string regexToLookFor, string ipv4AndPortToTarget, ExportType  exportType )
    {
        m_regexToLookFor = regexToLookFor;
        m_ipv4AndPortToTarget = ipv4AndPortToTarget;
        m_exportType =exportType;
    }
}

[System.Serializable]
public class RegexToUdpInterpreterContainerEvent: UnityEvent<RegexToUdpInterpreterContainer> { 

}