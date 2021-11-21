using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[System.Serializable]
public class StringToCommands
{
    public string m_startMatch = "";
    public List<ICommandLine> m_commands = new List<ICommandLine>();
    public StringToCommands(string startMatch, params string[] commands)
    {
        m_startMatch = startMatch;
        Add(commands);
    }

    public void Clear()
    {
        m_commands.Clear();
    }

    public void Add(params string[] commands)
    {
        for (int i = 0; i < commands.Length; i++)
        {
            m_commands.Add(new CommandLine(commands[i]));
        }
    }
    public bool IsMatching(string text)
    {
        return text.StartsWith(m_startMatch);
    }

    public void AddCommands(StringToCommands rc)
    {
        throw new NotImplementedException();
    }
}
