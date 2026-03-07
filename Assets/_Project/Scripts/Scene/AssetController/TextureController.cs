using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class TextureController : Singleton<TextureController>, ISaveable
{
    public int SaveLoadOrder { get; set; } = -1;

    public Dictionary<string, TextureSlot> Textures = new();
    public SceneEntity Entity;

    [SerializeField] private TextureMenu _textureMenu;

    private string _currentKey;

    private List<string> _textureKeys = new();

    private readonly ExtensionFilter[] _extensions = new [] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
    };
    private readonly string _defaultName = "Texture";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void LoadFile(string gameObjectName, string callbackMethod, string filter, bool multiple);
#endif

    protected override void Awake()
    {
        base.Awake();
    }

    public void OpenMenu(SceneEntity entity)
    {
        Entity = entity;
        _textureMenu.Open();
    }

    public void OpenMenu()
    {
        Entity = null;
        _textureMenu.Open();
    }

    public void SelectSlot(string key)
    {
        _currentKey = key;
    }

    public void ChangeTexture()
    {
        if (string.IsNullOrWhiteSpace(_currentKey))
        {
            return;
        }

        TextureSlot selectedTexture = Textures[_currentKey];

        selectedTexture.Texture.LoadImage(ReadFile());
    }

    public bool RemoveTexture()
    {
        if (string.IsNullOrWhiteSpace(_currentKey))
        {
            return false;
        }

        TextureSlot slot = Textures[_currentKey];
        Textures.Remove(_currentKey);
        slot.Remove();
        _currentKey = string.Empty;
        return true;
    }

    public void Apply()
    {
        if (string.IsNullOrWhiteSpace(_currentKey))
        {
            return;
        }

        Entity.SetTexture(_currentKey);
    }
    
    public void Clear()
    {
        Entity.SetTexture(string.Empty);        
    }

    public string UpdateKey(string currentKey, string newKey)
    {
        string key = GetUniqueKey(newKey);
        
        TextureSlot texture = Textures[currentKey];
        Textures.Remove(currentKey);
        Textures.Add(key, texture);

        return key;
    }

    public TextureSlot GetTexture(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        if (Textures.TryGetValue(key, out TextureSlot texture))
        {
            return texture;
        }
        return null;
    }

    public void AddTextureSlot()
    {
        TryChooseFromFile();
    }

    private void TryChooseFromFile()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            LoadFile(gameObject.name, nameof(OnFileUpload), "/image/*", false);
        #else
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", _extensions, false);
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
        texture.filterMode = FilterMode.Point;

        string key = GetUniqueKey(_defaultName);

        TextureSlot slot = new()
        {
            Texture = texture
        };

        Textures.Add(key, slot);
        _textureMenu.AddTextureSlotUI(key, texture);
    }

    private string GetUniqueKey(string baseName)
    {
        _textureKeys.Clear();
        _textureKeys.AddRange(Textures.Keys);

        return Utils.GenerateUniqueName(baseName, _textureKeys);
    }

    private byte[] ReadFile()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", _extensions, false);
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
        data.Textures.Clear();
        foreach (var item in Textures)
        {
            data.Textures.Add(new TextureData(item.Key, item.Value.Texture));
        }
    }

    public void LoadData(GameData data)
    {
        Textures.Clear();
        for (int i = 0; i < data.Textures.Count; i++)
        {
            TextureData textureData = data.Textures[i];
            Textures.Add(textureData.Key, new TextureSlot() { Texture = textureData.Texture });
        }
    }
}