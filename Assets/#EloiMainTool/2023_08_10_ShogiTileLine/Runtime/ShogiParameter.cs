using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;



[System.Serializable]
public class ShogiParameter
{
    public static Regex m_acceptedFormat = new Regex("(☗[A-Za-z0-9]+)(\\s[A-Za-z0-9_@./#&+-]+)?");
    [SerializeField] private string m_optionName = "";
    [SerializeField] private string m_value = "";

    public ShogiParameter()
    { }

    public ShogiParameter(string optionName) : this(optionName, "")
    {
    }

    public ShogiParameter(string optionName, string value)
    {
        if (optionName.Length > 0 && optionName[0] == '☗')
            optionName = optionName.Substring(1);
        this.m_optionName = optionName;
        this.m_value = value;
    }

    public string GetParamName() { return m_optionName; }
    public string GetParamNameWithShogit() { return "☗" + m_optionName; }
    public string GetParamNameAndParameter()
    {
        if (m_value.Trim().Length == 0)
            return "☗" + m_optionName;
        else return "☗" + m_optionName + " " + m_value;
    }
    public string GetValue() { return m_value; }

    public bool HasValue() { return string.IsNullOrEmpty(m_value); }
    public bool IsAtomic() { return !HasValue(); }

    public string GetAsString() { return string.Format("☗{0} {1}", m_optionName, m_value); }


    public static List<ShogiParameter> FindParametersInString(string processedOption)
    {
        List<ShogiParameter> result = new List<ShogiParameter>();
        MatchCollection collections = m_acceptedFormat.Matches(processedOption);
        for (int a = 0; a < collections.Count; a++)
        {
            string[] t = collections[a].Value.Split(' ');
            if (t.Length == 1)
                result.Add(new ShogiParameter(t[0]));
            if (t.Length == 2)
                result.Add(new ShogiParameter(t[0], t[1]));
        }
        return result;
    }

    public static bool HasParam(List<ShogiParameter> paramsToCheck, string paramToFound)
    {
        ShogiParameter tmp;
        return HasParam(paramsToCheck, paramToFound, out tmp);
    }
    public static bool HasParam(List<ShogiParameter> paramsToCheck, string paramToFound, out ShogiParameter found)
    {
        paramToFound = paramToFound.Replace("☗", "");
        found = null;
        if (paramsToCheck == null)
            return false;
        for (int i = 0; i < paramsToCheck.Count; i++)
        {
            if (paramsToCheck[i].GetParamName() == paramToFound)
            {
                found = paramsToCheck[i];
                return true;
            }
        }
        return false;
    }
}
