using System;
using System.Collections.Generic;
using Loykas.Scripting;

public static class Utils
{
    private static HashSet<int> _suffixes = new();
    
    public static string GenerateUniqueName(string baseName, List<string> exists)
    {
        _suffixes.Clear();
        
        int length = baseName.Length;

        int i;
        for (i = 0; i < exists.Count; i++)
        {
            string name = exists[i];
            if (name.StartsWith(baseName))
            {
                if (name.Length == length)
                {
                    _suffixes.Add(0);
                }
                else if (int.TryParse(name.AsSpan(length), out int number))
                {
                    _suffixes.Add(number);
                }
            }
        }

        i = 0;
        while (_suffixes.Contains(i))
        {
            i++;
        }

        return i == 0 ? baseName : string.Concat(baseName, i);
    }

    public static int ObjectToIndex(object value)
    {
        if (value is float floatValue)
        {
            return (int)floatValue;
        }
        else
        {
            return (int)value;
        }
    }

    public static ScriptNode GetNode(this List<ScriptNode> nodes, Guid id)
    {
        foreach (ScriptNode node in nodes)
        {
            if (node.ID == id)
            {
                return node;
            }
        }
        return null;
    }
}