using System;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UI;

public class AssetController : Singleton<AssetController>, ISaveable
{
    public int SaveLoadOrder { get; set; } = -1;
    public List<Texture2D> Textures => _textures;

    private List<Texture2D> _textures = new();
    private int _currentIndex = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SelectSlot(int index)
    {
        _currentIndex = index;
    }

    public bool AddTextureSlot(out int index, out Texture2D texture)
    {
        index = -1;
        if (!TryChooseFromFile(out texture))
        {
            return false;
        }

        index = _textures.Count;
        _textures.Add(texture);
        return true;
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

    private bool TryChooseFromFile(out Texture2D texture)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length == 0)
        {
            texture = null;
            return false;
        }

        string path = paths[0];
        byte[] data = File.ReadAllBytes(path);

        texture = new(1, 1);
        texture.LoadImage(data);
        texture.wrapMode = TextureWrapMode.Clamp;

        return true;
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