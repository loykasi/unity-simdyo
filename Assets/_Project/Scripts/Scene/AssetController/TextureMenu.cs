using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextureMenu : Singleton<TextureMenu>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _textureMenu;
    [SerializeField] private UITextureSlot _textureSlotPrefab;
    [SerializeField] private Transform _slotContainer;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _changeButton;

    private List<UITextureSlot> _textureSlots = new();

    private SceneEntity _entity;
    private bool _isMouseOver = false;
    private bool _isLoaded = false;

    protected override void Awake()
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

    private void Load()
    {
        if (_isLoaded)
        {
            return;
        }

        _isLoaded = true;
        Load(TextureController.Instance.Textures);
    }

    private void Load(List<Texture2D> textures)
    {
        foreach (var slot in _textureSlots)
        {
            Destroy(slot.gameObject);
        }
        _textureSlots.Clear();

        for (int i = 0; i < textures.Count; i++)
        {
            AddTextureSlotUI(i, textures[i]);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }

    public void OpenTextureMenu(SceneEntity entity)
    {
        Load();

        _entity = entity;
        _textureMenu.SetActive(true);
    }

    public void CloseTextureMenu()
    {
        _entity = null;
        _textureMenu.SetActive(false);
    }

    public void AddTextureSlotUI(int index, Texture2D texture)
    {
        var textureSlot = Instantiate(_textureSlotPrefab, _slotContainer);
        textureSlot.Init(index, texture);
        _textureSlots.Add(textureSlot);
    }

    public void SelectSlot(int index)
    {
        TextureController.Instance.SelectSlot(index);

        for (int i = 0; i < _textureSlots.Count; i++)
        {
            if (_textureSlots[i].Index == index)
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

    public void AddTextureSlot()
    {
        TextureController.Instance.AddTextureSlot(AddTextureSlotUI);
    }

    public void ChangeTexture()
    {
        TextureController.Instance.ChangeTexture();
    }

    
    public void Apply()
    {
        if (!TextureController.Instance.Apply(out int index))
        {
            return;
        }
        
        _entity.SetTexture(index);
        CloseTextureMenu();
    }
}