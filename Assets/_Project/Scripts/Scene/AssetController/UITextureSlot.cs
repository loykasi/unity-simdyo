using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextureSlot : MonoBehaviour, IPointerClickHandler
{
    public string Key;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RawImage _image;
    [SerializeField] private TMP_InputField _input;

    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defaultColor;

    private TextureMenu _textureMenu;

    private void Awake()
    {
        _input.onEndEdit.AddListener(OnEndEdit);
    }

    public void Init(TextureMenu menu, string key, Texture2D texture)
    {
        Key = key;
        _image.texture = texture;
        _input.text = key;

        _textureMenu = menu;
    }

    private void OnEndEdit(string value)
    {
        if (value == Key)
        {
            return;
        }
        
        Key = _textureMenu.UpdateKey(Key, value);
        _input.text = Key;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _textureMenu.SelectSlot(Key);
    }

    public void Select()
    {
        _backgroundImage.color = _selectColor;
    }

    public void DeSelect()
    {
        _backgroundImage.color = _defaultColor;
    }
}