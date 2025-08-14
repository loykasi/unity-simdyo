using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ColorPropertyPicker : ColorProperty
{
    [SerializeField] protected RawImage _image;
    [SerializeField] protected TMP_InputField _input;
    [SerializeField] protected Slider _slider;
    [SerializeField] protected int _range;
    protected Texture2D _texture;
    protected float _value;

    private void Awake()
    {
        CreateImage();
    }

    private void CreateImage()
    {
        _texture = new Texture2D(16, 1)
        {
            wrapMode = TextureWrapMode.Clamp,
        };

        UpdateTexture();
        _image.texture = _texture;
    }

    protected abstract void UpdateTexture();

    public void UpdateFromSlider(float value)
    {
        _input.text = value.ToString();
        _value = value / _range;

        UpdateColor();
    }

    public void UpdateFromInput(string input)
    {
        if (int.TryParse(input, out int value))
        {
            value = Mathf.Clamp(value, 0, _range);
            _value = value;
            _slider.SetValueWithoutNotify(_value);
        }
        _input.text = _value.ToString();
        _value /= _range;

        UpdateColor();
    }

    protected abstract void UpdateColor();
}