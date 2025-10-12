using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Utils
{
    public static string GenerateUniqueName(string baseName, List<string> exists)
    {
        string pattern = @$"^{baseName}(?: \((\d+)\))?$";

        Regex regex = new(pattern, RegexOptions.Compiled);

        List<int> ints = new();

        int i = 0;

        for (i = 0; i < exists.Count; i++)
        {
            Match match = regex.Match(exists[i]);
            if (match.Success)
            {
                string value = match.Groups[1].Value;
                int number = value == string.Empty ? 0 : int.Parse(value);
                ints.Add(number);
            }
        }
        ints.Sort();

        for (i = 0; i < ints.Count; i++)
        {
            if (i != ints[i])
            {
                break;
            }
        }

        if (i == 0)
        {
            return baseName;
        }
        return string.Concat(baseName, " (", i, ")");
    }
}