using System.Collections.Generic;

public class TemporaryEnvironment
{
    private Dictionary<string, string> _variables = new();

    public void SetVariable(string key, string value)
    {
        _variables.Add(key, value);
    }

    public string GetVariable(string key)
    {
        _variables.TryGetValue(key, out string value);
        return value;
    }
}