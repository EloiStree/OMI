using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class QD_BiikeabToJOMIFromFiles : MonoBehaviour
{

    [Header("Regex to JOMI")]
    public RegexBooleanToActionObserved m_regexToJomi;
    public ObservedSyncFile m_regexToJomiFile;
 
    [Header("To Code later")]
    public TextAsset m_booleansRegexLnToJomi;
    public TextAsset m_booleansRegexNlToJomi;
    public TextAsset m_linearStateMachineToJomi;


   

 
   
  
    

  
   
}



public class ActionAsString {
    public string m_action;

    public ActionAsString(string shortcut)
    {
        m_action = shortcut;
    }
}
