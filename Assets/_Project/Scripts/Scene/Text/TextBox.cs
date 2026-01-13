using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    public GameObject TextFieldBox;
    public RectTransform RectTransform;
    public TMP_Text TextField;

    public string Text
    {
        get => TextField.text;
        set
        {
            TextField.text = value;

            bool active = value.Length > 0;
            if (TextFieldBox.activeSelf != active)
            {
                RectTransform.gameObject.SetActive(active);   
            }
        }
    }

    public Color Color
    {
        get => TextField.color;
        set => TextField.color = value;
    }

    public float Size
    {
        get => TextField.fontSize;
        set => TextField.fontSize = value;
    }

    public HorizontalAlignmentOptions HorizontalAlignment
    {
        get => TextField.horizontalAlignment;
        set => TextField.horizontalAlignment = value;
    }

    public VerticalAlignmentOptions VerticalAlignment
    {
        get => TextField.verticalAlignment;
        set => TextField.verticalAlignment = value;
    }

    public void Resize(float width, float height)
    {
        RectTransform.sizeDelta = new Vector2(width, height);
    }
}