using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataService : IDataService
{
    private ISerializer _serializer;

    private readonly string _dataPath = "C:\\Users\\Admin\\Desktop\\_\\playground";
    private readonly string _extension = "json";

    public DataService(ISerializer serializer)
    {
        _serializer = serializer;
    }

    private string GetPath(string name)
    {
        return Path.Combine(_dataPath, string.Concat(name, ".", _extension));
    }

    public void Save(GameData data)
    {
        string path = GetPath(data.Name);
        try
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            File.WriteAllText(path, _serializer.Serialize(data));
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            throw new IOException("Error in saving data");
        }
    }

    public GameData Load(string name)
    {
        string path = GetPath(name);

        if (!File.Exists(path))
        {
            throw new ArgumentException($"No save data '{name}'");
        }

        return _serializer.Deserialize<GameData>(File.ReadAllText(path));
    }
}