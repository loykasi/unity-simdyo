using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextureMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _textureMenu;
    [SerializeField] private UITextureSlot _textureSlotPrefab;
    [SerializeField] private Transform _slotContainer;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _changeButton;
    [SerializeField] private Button _removeButton;

    private List<UITextureSlot> _textureSlots = new();
    private int _selectedIndex;

    private bool _isMouseOver = false;
    private bool _isLoaded = false;

    private void Awake()
    {
        _changeButton.interactable = false;
        _applyButton.interactable = false;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isMouseOver)
        {
            CloseTextureMenu();
        }
    }

    public void Open()
    {
        Load();

        _applyButton.gameObject.SetActive(TextureController.Instance.Entity != null);
        _textureMenu.SetActive(true);
    }

    private void Load()
    {
        if (_isLoaded)
        {
            return;
        }

        _isLoaded = true;
        Load(TextureController.Instance.Textures);
    }

    private void Load(Dictionary<string, Texture2D> textures)
    {
        foreach (var slot in _textureSlots)
        {
            Destroy(slot.gameObject);
        }
        _textureSlots.Clear();

        foreach (var slot in textures)
        {
            AddTextureSlotUI(slot.Key, slot.Value);
        }
    }

    public void AddTextureSlotUI(string key, Texture2D texture)
    {
        var textureSlot = Instantiate(_textureSlotPrefab, _slotContainer);
        textureSlot.Init(this, key, texture);
        _textureSlots.Add(textureSlot);
    }

    public void AddTextureSlot()
    {
        TextureController.Instance.AddTextureSlot();
    }

    public void CloseTextureMenu()
    {
        _textureMenu.SetActive(false);
    }

    public void SelectSlot(string key)
    {
        TextureController.Instance.SelectSlot(key);

        for (int i = 0; i < _textureSlots.Count; i++)
        {
            if (_textureSlots[i].Key == key)
            {
                _textureSlots[i].Select();
                _selectedIndex = i;
            }
            else
            {
                _textureSlots[i].DeSelect();
            }
        }

        _changeButton.interactable = true;
        _removeButton.interactable = true;
        _applyButton.interactable = true;
    }

    public void ChangeTexture()
    {
        TextureController.Instance.ChangeTexture();
    }

    public void RemoveSlot()
    {
        if (TextureController.Instance.RemoveTexture())
        {
            UITextureSlot slot = _textureSlots[_selectedIndex];
            _textureSlots.RemoveAt(_selectedIndex);
            Destroy(slot.gameObject);

            _selectedIndex = -1;
            _changeButton.interactable = false;
            _removeButton.interactable = false;
            _applyButton.interactable = false;
        }
    }

    
    public void Apply()
    {
        TextureController.Instance.Apply();
        CloseTextureMenu();
    }

    public void Clear()
    {
        TextureController.Instance.Clear();
        CloseTextureMenu();
    }

    public string UpdateKey(string currentKey, string newKey)
    {
        return TextureController.Instance.UpdateKey(currentKey, newKey);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }
}