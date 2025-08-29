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

    [SerializeField] private UITextureSlot _textureSlotPrefab;
    [SerializeField] private Transform _slotContainer;
    [SerializeField] private GameObject _textureMenu;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _changeButton;

    private List<Texture2D> _textures = new();
    private List<UITextureSlot> _textureSlots = new();
    private SceneEntity _entity;
    private int _currentIndex = 1;

    protected override void Awake()
    {
        base.Awake();

        _changeButton.interactable = false;
        _applyButton.interactable = false;
    }

    public void OpenTextureMenu(SceneEntity entity)
    {
        _entity = entity;
        _textureMenu.SetActive(true);
    }

    public void CloseTextureMenu()
    {
        _entity = null;
        _textureMenu.SetActive(false);
    }

    public void AddTextureSlot()
    {
        if (!TryChooseFromFile(out Texture2D texture))
        {
            return;
        }

        int index = _textures.Count;
        AddTextureSlotUI(index, texture);
        _textures.Add(texture);
    }

    private void AddTextureSlotUI(int index, Texture2D texture)
    {
        var textureSlot = Instantiate(_textureSlotPrefab, _slotContainer);
        textureSlot.Init(index, texture);
        _textureSlots.Add(textureSlot);
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

    public void SelectSlot(int index)
    {
        _currentIndex = index;

        for (int i = 0; i < _textureSlots.Count; i++)
        {
            if (_textureSlots[i].Index == _currentIndex)
            {
                _textureSlots[i].Select();
            }
            else
            {
                _textureSlots[i].DeSelect();
            }
        }

        _changeButton.interactable = true;
        _applyButton.interactable = true;
    }

    public void Apply()
    {
        if (_currentIndex == -1)
        {
            return;
        }

        Texture2D texture = _textures[_currentIndex];
        _entity.SetTexture(_currentIndex, texture);

        CloseTextureMenu();
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
        foreach (var slot in _textureSlots)
        {
            Destroy(slot.gameObject);
        }
        _textureSlots.Clear();

        for (int i = 0; i < data.Textures.Count; i++)
        {
            AddTextureSlotUI(i, data.Textures[i]);
        }
    }
}