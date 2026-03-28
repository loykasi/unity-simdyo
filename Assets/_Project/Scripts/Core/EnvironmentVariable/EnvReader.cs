using System.IO;
using UnityEngine;

public static class EnvReader
{
    public static TemporaryEnvironment Load(string filePath)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The file '{filePath}' does not exist.");   
        }

        TemporaryEnvironment environment = new();

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                continue;
            }

            string[] parts = line.Split('=', 2);
            if (parts.Length != 2)
            {
                continue;
            }

            string key = parts[0].Trim();
            string value = parts[1].Trim();
            environment.SetVariable(key, value);
        }

        return environment;
    }
}
