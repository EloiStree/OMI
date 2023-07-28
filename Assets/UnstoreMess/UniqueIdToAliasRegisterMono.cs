using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdToAliasRegisterMono : MonoBehaviour
{

    public GroupOfUniqueIdToAlias m_groupOfAlias;
    public bool m_ignoreCase=true;

    public void AddComToAlias(string com, params string[] alias)
    {
        m_groupOfAlias.GetById(com, m_ignoreCase, out bool found, out UniqueIdToListOfAlias link);
        if (found)
            link.m_aliasName.AddRange(alias);
        else m_groupOfAlias.m_registeredIdToAlias.Add(new UniqueIdToListOfAlias(com, alias));

    }
    public void AddComToAlias(string com, IEnumerable<string> alias)
    {
        m_groupOfAlias.GetById(com, m_ignoreCase, out bool found, out UniqueIdToListOfAlias link);
        if (found)
            link.m_aliasName.AddRange(alias);
        else m_groupOfAlias.m_registeredIdToAlias.Add(new UniqueIdToListOfAlias(com, alias));

    }
}


[System.Serializable]
public class  GroupOfUniqueIdToAlias
{
    public List<UniqueIdToListOfAlias> m_registeredIdToAlias= new List<UniqueIdToListOfAlias>();

    public void GetAny(string lookingFor, bool ignoreCase, out bool found, out UniqueIdToListOfAlias link, out bool isCom)
    {

        for (int i = 0; i < m_registeredIdToAlias.Count; i++)
        {
            if (Eloi.E_StringUtility.AreEquals(m_registeredIdToAlias[i].m_uniqueId, lookingFor, ignoreCase, true))
            {
                found = true;
                isCom = true;
                link = m_registeredIdToAlias[i];
                return;
            }
            for (int j = 0; j < m_registeredIdToAlias[i].m_aliasName.Count; j++)
            {
                if (Eloi.E_StringUtility.AreEquals(m_registeredIdToAlias[i].m_aliasName[j], lookingFor, ignoreCase, true))
                {
                    found = true;
                    isCom = false;
                    link = m_registeredIdToAlias[i];
                    return;
                }
            }
        }

        found = false;
        isCom = false;
        link = null;
    }
    public void GetById(string lookingFor, bool ignoreCase, out bool found, out UniqueIdToListOfAlias link)
    {

        for (int i = 0; i < m_registeredIdToAlias.Count; i++)
        {
            if (Eloi.E_StringUtility.AreEquals(m_registeredIdToAlias[i].m_uniqueId, lookingFor, ignoreCase, true))
            {
                found = true;
                link = m_registeredIdToAlias[i];
                return;
            }

        }

        found = false;
        link = null;
    }
    public void GetByAlias(string lookingFor, bool ignoreCase, out bool found, out UniqueIdToListOfAlias link)
    {

        for (int i = 0; i < m_registeredIdToAlias.Count; i++)
        {
            
            for (int j = 0; j < m_registeredIdToAlias[i].m_aliasName.Count; j++)
            {
                if (Eloi.E_StringUtility.AreEquals(m_registeredIdToAlias[i].m_aliasName[j], lookingFor, ignoreCase, true))
                {
                    found = true;
                    link = m_registeredIdToAlias[i];
                    return;
                }
            }
        }

        found = false;
        link = null;
    }

}
[System.Serializable]
public class UniqueIdToListOfAlias
{


    public string m_uniqueId="";
    public List<string> m_aliasName= new List<string>();

    public UniqueIdToListOfAlias()
    {
    }

    public UniqueIdToListOfAlias(string comId, IEnumerable<string> aliasName)
    {
        m_uniqueId = comId;
        m_aliasName.AddRange(aliasName);
    }
}


