using UnityEngine;

public class HuePicker : ColorPropertyPicker
{
    public float Hue => ColorPickerController.Instance.Hue;

    protected override void UpdateTexture()
    {
        for (int i = 0; i < _texture.width; i++)
        {
            _texture.SetPixel(i, 0, Color.HSVToRGB((float)i / _texture.width, 1, 1));
        }

        _texture.Apply();
    }

    protected override void UpdateColor()
    {
        ColorPickerController.Instance.Hue = _value;
    }

    public override void Init()
    {
        _slider.SetValueWithoutNotify(Hue * _range);
        _input.text = (Hue * _range).ToString();
    }
}