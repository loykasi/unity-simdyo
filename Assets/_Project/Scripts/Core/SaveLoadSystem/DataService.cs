using System;
using System.IO;
using UnityEngine;
using System.IO.Compression;
using SFB;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;

public class DataService : MonoBehaviour, IDataService
{
    private ISerializer _serializer = new JsonSerializer();

    private readonly string _debugPath = "C:\\Users\\Admin\\Desktop\\_\\playground\\unity";
    private string _path;
    private GameData _data;
    private UnityAction _callback;

    private readonly string _defaultName = "SceneProject";
    private readonly string _dataExtension = ".zip";
    private readonly string _sceneExtension = ".json";
    private readonly string _textureExtension = ".png";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SaveFile(string filename, byte[] byteArray, int byteArraySize);

    [DllImport("__Internal")]
    private static extern void LoadFile(string gameObjectName, string callbackMethod, string filter, bool multiple);
#endif

    public void Save(GameData data)
    {
        try
        {
            byte[] bytes = CreateSaveData(data);

#if UNITY_WEBGL && !UNITY_EDITOR
            
            SaveFile(_defaultName, bytes, bytes.Length);

#else

            string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "zip");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, bytes);

            ToastSystem.Instance.Show($"Save as\n{path}");

#endif
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            throw new IOException("Error in saving data");
        }
    }

    private byte[] CreateSaveData(GameData data)
    {
        using MemoryStream memoryStream = new();
        using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
        {
            // main data
            var entry = archive.CreateEntry("scene.json", System.IO.Compression.CompressionLevel.NoCompression);
            using (Stream stream = entry.Open())
            {
                using StreamWriter streamWriter = new(stream);
                string serializedData = _serializer.Serialize(data.Scene);
                streamWriter.Write(serializedData);

                // Debug purpose
                // File.WriteAllText
                // (
                //     Path.Combine(_debugPath, string.Concat("scene", _sceneExtension)),
                //     serializedData
                // );
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
        }

        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }

    public void Load(string path, GameData data, UnityAction callback)
    {
        _callback = callback;
        if (!File.Exists(path))
        {
            throw new ArgumentException($"No save data");
        }
        
        using ZipArchive archive = ZipFile.OpenRead(path);
        LoadToGameData(archive, data);
    }

    public void Load(GameData data, UnityAction callback)
    {
        _callback = callback;
        #if UNITY_WEBGL && !UNITY_EDITOR
            _data = data;
            LoadFile(gameObject.name, nameof(OnFileUpload), ".zip", false);
        #else
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
            if (paths.Length == 0)
            {
                throw new ArgumentException($"No save data");
            }

            string path = paths[0];

            if (!File.Exists(path))
            {
                throw new ArgumentException($"No save data");
            }
            
            using ZipArchive archive = ZipFile.OpenRead(path);
            LoadToGameData(archive, data);
        #endif
    }

    private void LoadToGameData(ZipArchive archive, GameData data)
    {
        try
        {
            if (data.Textures != null)
            {
                data.Textures.Clear();
            }
            else
            {
                data.Textures = new();
            }

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

            _callback();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            throw new IOException("Error in loading data");
        }
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    public void OnFileUpload(string json)
    {
        string[] urls = JsonUtils.FromJson<string>(json);
        StartCoroutine(LoadFromUrl(urls[0], OnFileLoaded));
    }

    private IEnumerator LoadFromUrl(string url, UnityAction<byte[]> callback = null) {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] data = new byte[www.downloadHandler.data.Length];
                Array.Copy(www.downloadHandler.data, 0, data, 0, www.downloadHandler.data.Length);

                callback?.Invoke(data);
            }
            else
            {
                Debug.Log($"Failed to load {url}: {www.error}");
            }
        }
    }

    private void OnFileLoaded(byte[] bytes)
    {
        using MemoryStream memoryStream = new(bytes);
        using ZipArchive archive = new(memoryStream, ZipArchiveMode.Read);
        LoadToGameData(archive, _data);
    }
#else
    private ZipArchive LoadSaveData()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length == 0)
        {
            throw new ArgumentException($"No save data");
        }

        string path = paths[0];

        if (!File.Exists(path))
        {
            throw new ArgumentException($"No save data");
        }

        return ZipFile.OpenRead(path);
    }
#endif
}
