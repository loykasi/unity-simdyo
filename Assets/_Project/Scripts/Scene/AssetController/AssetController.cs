using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class AssetController : Singleton<AssetController>, ISaveable
{
    public int SaveLoadOrder { get; set; } = -1;
    public List<Texture2D> Textures => _textures;

    private List<Texture2D> _textures = new();
    private int _currentIndex = 1;

    private UnityAction<int, Texture2D> _addTextureCallback;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void LoadFile(string gameObjectName, string callbackMethod, string filter, bool multiple);
#endif

    protected override void Awake()
    {
        base.Awake();
    }

    public void SelectSlot(int index)
    {
        _currentIndex = index;
    }

    public void ChangeTexture()
    {
        if (_currentIndex == -1)
        {
            return;
        }

        Texture2D selectedTexture = _textures[_currentIndex];

        selectedTexture.LoadImage(ReadFile());
    }

    
    public bool Apply(out int index, out Texture2D texture)
    {
        index = -1;
        texture = null;
        if (_currentIndex == -1)
        {
            return false;
        }

        texture = _textures[_currentIndex];
        return true;
    }

    public void AddTextureSlot(UnityAction<int, Texture2D> callback)
    {
        _addTextureCallback = callback;
        TryChooseFromFile();
    }

    private void TryChooseFromFile()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            LoadFile(gameObject.name, nameof(OnFileUpload), "/image/*", false);
        #else
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
            if (paths.Length == 0)
            {
                return;
            }

            string path = paths[0];
            byte[] data = File.ReadAllBytes(path);

            OnFileLoaded(data);
        #endif
    }

    public void OnFileUpload(string json)
    {
        string[] urls = JsonUtils.FromJson<string>(json);
        StartCoroutine(LoadFromUrl(urls[0], OnFileLoaded));
    }

    private IEnumerator LoadFromUrl(string url, UnityAction<byte[]> callback = null)
    {
        using UnityWebRequest www = UnityWebRequest.Get(url);
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

    private void OnFileLoaded(byte[] bytes)
    {
        Texture2D texture = new(1, 1);
        texture.LoadImage(bytes);
        texture.wrapMode = TextureWrapMode.Clamp;

        int index = _textures.Count;
        _textures.Add(texture);

        _addTextureCallback?.Invoke(index, texture);
    }

    private byte[] ReadFile()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length == 0)
        {
            return Array.Empty<byte>();
        }

        string path = paths[0];
        byte[] data = File.ReadAllBytes(path);

        return data;
    }

    public void SaveData(GameData data)
    {
        data.Textures = Textures;
    }

    public void LoadData(GameData data)
    {
        _textures = data.Textures;
    }
}