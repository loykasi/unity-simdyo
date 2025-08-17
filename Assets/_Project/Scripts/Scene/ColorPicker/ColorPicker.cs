using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : ColorProperty, IDragHandler, IPointerDownHandler
{
    public float Hue => ColorPickerController.Instance.Hue;
    public float Saturation => ColorPickerController.Instance.Saturation;
    public float Value => ColorPickerController.Instance.Value;

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _pickerTransform;
    [SerializeField] private RawImage _image;
    private Texture2D _texture;

    private void Awake()
    {
        CreateImage();
    }

    private void OnEnable()
    {
        ColorPickerController.Instance.OnColorUpdated += OnColorUpdated;
    }

    private void OnDisable()
    {
        if (ColorPickerController.Instance == null) return;
        ColorPickerController.Instance.OnColorUpdated -= OnColorUpdated;
    }

    private void OnColorUpdated(float hue, float saturation, float value, float alpha)
    {
        UpdateTexture();
        UpdatePicker();
    }

    private void CreateImage()
    {
        _texture = new Texture2D(16, 16)
        {
            wrapMode = TextureWrapMode.Clamp,
            name = "Color"
        };

        for (int y = 0; y < _texture.height; y++)
        {
            for (int x = 0; x < _texture.width; x++)
            {
                _texture.SetPixel(
                    x,
                    y,
                    Color.HSVToRGB
                    (
                        Hue,
                        (float)x / _texture.width,
                        (float)y / _texture.height
                    )
                );
            }
        }

        _texture.Apply();

        _image.texture = _texture;
    }

    private void UpdateTexture()
    {
        for (int y = 0; y < _texture.height; y++)
        {
            for (int x = 0; x < _texture.width; x++)
            {
                _texture.SetPixel(
                    x,
                    y,
                    Color.HSVToRGB
                    (
                        Hue,
                        (float)x / _texture.width,
                        (float)y / _texture.height
                    )
                );
            }
        }

        _texture.Apply(); 
    }

    private void UpdatePicker()
    {
        Vector3 pos = new Vector3
        (
            Saturation * _rectTransform.sizeDelta.x,
            (Value - 1) * _rectTransform.sizeDelta.y
        );
        _pickerTransform.localPosition = pos;
    }

    private void UpdateValue(Vector3 mousePosition)
    {
        // pivot is top-left
        Vector3 pos = _rectTransform.InverseTransformPoint(mousePosition);

        Vector2 sizeDelta = _rectTransform.sizeDelta;

        pos.x = Mathf.Clamp(pos.x, 0, sizeDelta.x);
        pos.y = Mathf.Clamp(pos.y, -sizeDelta.y, 0);

        float saturation = pos.x / _rectTransform.sizeDelta.x;
        float value = 1 + (pos.y / _rectTransform.sizeDelta.y);

        saturation = Mathf.Round(saturation * 100) / 100f;
        value = Mathf.Round(value * 100) / 100f;
        ColorPickerController.Instance.UpdateSaturationAndValue(saturation, value);

        _pickerTransform.localPosition = pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateValue(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateValue(eventData.position);
    }

    public override void Init()
    {
        UpdateTexture();
        UpdatePicker();
    }
}