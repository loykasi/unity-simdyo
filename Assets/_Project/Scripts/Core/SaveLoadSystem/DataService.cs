using System;
using System.IO;
using UnityEngine;
using System.IO.Compression;

public class DataService : IDataService
{
    private ISerializer _serializer;

    private readonly string _dataPath = "C:\\Users\\Admin\\Desktop\\_\\playground\\unity";
    private string _path;

    private readonly string _dataExtension = ".zip";
    private readonly string _sceneExtension = ".json";
    private readonly string _textureExtension = ".png";

    public DataService(ISerializer serializer)
    {
        _serializer = serializer;
    }

    public void SetPath(string path)
    {
        _path = path;
    }

    private string GetPath(string name)
    {
        return _path;
        // return Path.Combine(_path, string.Concat(name, _dataExtension));
    }

    public void Save(string name, GameData data)
    {
        string path = GetPath(name);
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Create);

            // main data
            var entry = archive.CreateEntry("scene.json", System.IO.Compression.CompressionLevel.NoCompression);
            using (Stream stream = entry.Open())
            {
                using StreamWriter streamWriter = new(stream);
                string serializedData = _serializer.Serialize(data.Scene);
                streamWriter.Write(serializedData);

                // Debug purpose
                File.WriteAllText
                (
                    Path.Combine(_dataPath, string.Concat("scene", _sceneExtension)),
                    serializedData
                );
            }

            var textureFolder = archive.CreateEntry("textures/");

            // textures
            for (int i = 0; i < data.Textures.Count; i++)
            {
                var texture = data.Textures[i];
                var textureEntry = archive.CreateEntry($"textures/{i + 1}.png", System.IO.Compression.CompressionLevel.NoCompression);
                using Stream texturestream = textureEntry.Open();
                using BinaryWriter binaryWriter = new(texturestream);
                binaryWriter.Write(texture.EncodeToPNG());
            }

            ToastSystem.Instance.Show($"Save as\n{path}");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            throw new IOException("Error in saving data");
        }
    }

    public void Load(string name, GameData data)
    {
        if (data.Textures != null)
        {
            data.Textures.Clear();
        }
        else
        {
            data.Textures = new();
        }

        string path = GetPath(name);

        if (!File.Exists(path))
        {
            throw new ArgumentException($"No save data '{name}'");
        }

        // open zip
        using ZipArchive archive = ZipFile.OpenRead(path);

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            if (entry.FullName.EndsWith(_sceneExtension, StringComparison.OrdinalIgnoreCase))
            {
                using Stream stream = entry.Open();
                using StreamReader reader = new(stream);
                data.Scene = _serializer.Deserialize<SceneData>(reader.ReadToEnd());
            }
            else if (entry.FullName.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            else if (entry.Name.EndsWith(_textureExtension, StringComparison.OrdinalIgnoreCase))
            {
                string fileName = Path.GetFileNameWithoutExtension(entry.Name);
                if (int.TryParse(fileName, out int index))
                {
                    using Stream stream = entry.Open();
                    using MemoryStream memory = new();
                    stream.CopyTo(memory);
                    byte[] textureData = memory.ToArray();

                    Texture2D texture = new(2, 2);
                    texture.LoadImage(textureData);

                    data.Textures.Add(texture);
                }
            }
        }
    }
}