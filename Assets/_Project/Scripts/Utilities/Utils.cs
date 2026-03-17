using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static string GenerateUniqueName(string baseName, List<string> exists)
    {
        HashSet<int> suffixes = new();
        int length = baseName.Length;

        int i;
        for (i = 0; i < exists.Count; i++)
        {
            string name = exists[i];
            if (name.StartsWith(baseName))
            {
                if (name.Length == length)
                {
                    suffixes.Add(0);
                }
                else if (int.TryParse(name.AsSpan(length), out int number))
                {
                    suffixes.Add(number);
                }
            }
        }

        i = 0;
        while (suffixes.Contains(i))
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
}