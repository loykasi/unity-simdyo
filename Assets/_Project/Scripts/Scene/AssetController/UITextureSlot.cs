using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextureSlot : MonoBehaviour, IPointerClickHandler
{
    public int Index;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RawImage _image;
    [SerializeField] private TextMeshProUGUI _label;

    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defaultColor;

    public void Init(int index, Texture2D texture)
    {
        Index = index;
        _image.texture = texture;
        _label.text = string.Concat("#", index + 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TextureMenu.Instance.SelectSlot(Index);
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