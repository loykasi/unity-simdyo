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

    private UnityAction _onSuccess;
    private UnityAction _onFailure;

    private readonly string _defaultName = "project";
    private readonly string _dataExtension = "simdyo";
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
            
            SaveFile($"{_defaultName}.{_dataExtension}", bytes, bytes.Length);

#else

            string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", _defaultName, _dataExtension);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, bytes);

            ToastSystem.Instance.Show($"Save as\n{path}");

#endif
        }
        catch (Exception)
        {
            // Debug.Log(ex.Message);
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

            // textures
            var textureFolder = archive.CreateEntry("textures/");
            for (int i = 0; i < data.Textures.Count; i++)
            {
                var texture = data.Textures[i];
                var textureEntry = archive.CreateEntry($"textures/{texture.Key}.png", System.IO.Compression.CompressionLevel.NoCompression);
                using Stream texturestream = textureEntry.Open();
                using BinaryWriter binaryWriter = new(texturestream);
                binaryWriter.Write(texture.Texture.EncodeToPNG());
            }

            // thumbnails
            {
                var thumbnailEntry = archive.CreateEntry("thumbnail.png", System.IO.Compression.CompressionLevel.NoCompression);
                using Stream texturestream = thumbnailEntry.Open();
                using BinaryWriter binaryWriter = new(texturestream);
                byte[] bytes = ThumbnailRender.Instance.GetThumbnail();
                binaryWriter.Write(bytes);
            }
        }

        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }

    public void Load(GameData data, UnityAction onSuccess, UnityAction onFailure)
    {
        _onSuccess = onSuccess;
        _onFailure = onFailure;
        
        #if UNITY_WEBGL && !UNITY_EDITOR
            _data = data;
            LoadFile(gameObject.name, nameof(OnFileUploadFromBrowser), $".{_dataExtension}", false);
        #else
            LoadFileFromDesktop(data);
        #endif
    }

    private void LoadFileFromDesktop(GameData data)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", _dataExtension, false);
        if (paths.Length == 0)
        {
            return;
        }

        string path = paths[0];
        if (!File.Exists(path))
        {
            _onFailure?.Invoke();
            
            throw new ArgumentException($"Path not exits");
        }

        try
        {
            using ZipArchive archive = ZipFile.OpenRead(path);
            LoadProject(archive, data);
        }
        catch (Exception exception)
        {
            OnLoadFailed(exception.Message);
        }
    }

    public void OnFileUploadFromBrowser(string json)
    {
        string[] urls = JsonUtils.FromJson<string>(json);
        StartCoroutine(LoadFromUrl(urls[0]));
    }

    public void LoadFromUrl(string url, GameData data, UnityAction onSuccess, UnityAction onFailure)
    {
        _onSuccess = onSuccess;
        _onFailure = onFailure;

        // window.location.pathname.replace(/^\/|\/$/g, '').split("/").pop()
        _data = data;
        StartCoroutine(LoadFromUrl(url));
    }

    private IEnumerator LoadFromUrl(string url)
    {
        using UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        HandleDownloadData(www);
    }

    private void HandleDownloadData(UnityWebRequest www)
    {
        if (www.result != UnityWebRequest.Result.Success)
        {
            OnLoadFailed(www.error);
            return;
        }

        try
        {
            // byte[] data = new byte[www.downloadHandler.data.Length];
            // Array.Copy(www.downloadHandler.data, 0, data, 0, www.downloadHandler.data.Length);
            
            using MemoryStream memoryStream = new(www.downloadHandler.data);
            using ZipArchive archive = new(memoryStream, ZipArchiveMode.Read);
            LoadProject(archive, _data);
        }
        catch (Exception exception)
        {
            OnLoadFailed(exception.Message);
        }
    }

    private void OnLoadFailed(string exception = null)
    {
        Debug.LogError($"Failed to load with error: {exception}");

        ToastSystem.Instance.Show($"Load project failed");
        _onFailure?.Invoke();
    }

    private void LoadProject(ZipArchive archive, GameData data)
    {
        try
        {
            LoadToGameData(archive, data);
            _onSuccess?.Invoke();
        }
        catch (Exception)
        {
            throw;
        }
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
                else if (entry.Name.EndsWith(_textureExtension, StringComparison.OrdinalIgnoreCase) &&
                        entry.FullName.Contains("textures/"))
                {
                    string key = Path.GetFileNameWithoutExtension(entry.Name);

                    using Stream stream = entry.Open();
                    using MemoryStream memory = new();
                    stream.CopyTo(memory);
                    byte[] textureData = memory.ToArray();

                    Texture2D texture = new(2, 2)
                    {
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Point
                    };
                    texture.LoadImage(textureData);

                    data.Textures.Add(new TextureData(key, texture));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


// #if UNITY_WEBGL && !UNITY_EDITOR
//     public void OnFileUpload(string json)
//     {
//         string[] urls = JsonUtils.FromJson<string>(json);
//         StartCoroutine(LoadFromUrl(urls[0], OnFileLoaded));
//     }
// #else
//     private ZipArchive LoadSaveData()
//     {
//         var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
//         if (paths.Length == 0)
//         {
//             throw new ArgumentException($"No save data");
//         }

//         string path = paths[0];

//         if (!File.Exists(path))
//         {
//             throw new ArgumentException($"No save data");
//         }

//         return ZipFile.OpenRead(path);
//     }
// #endif
}
