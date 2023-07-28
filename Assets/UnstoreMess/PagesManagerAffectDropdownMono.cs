using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PagesManagerAffectDropdownMono : MonoBehaviour
{
    public PagesManagerMono m_linkedManager;
    public Dropdown [] m_dropdownToAffect;

    public void IndexFromDropdownChoosed(int newIndexChoosedOfDropdown) {
        for (int i = 0; i < m_dropdownToAffect.Length; i++)
        {
            m_dropdownToAffect[i].SetValueWithoutNotify(newIndexChoosedOfDropdown);
        }
        m_linkedManager.DisplayOnlyAtIndex(newIndexChoosedOfDropdown);
    }

    [ContextMenu("Refresh")]
    public void Refresh() {
        for (int i = 0; i < m_dropdownToAffect.Length; i++)
        {
            m_dropdownToAffect[i].ClearOptions();
            m_dropdownToAffect[i].AddOptions(m_linkedManager.m_pages.Select(k => k.m_displayInScroll).ToList());
        }
    }
}
