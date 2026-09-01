using UnityEngine;
using UnityEngine.UI;

public class UIToolManager : MonoBehaviour
{
    [Header("Rotate Tool")]
    public RectTransform _visualization;
    public Material _rotationMaterial;
    public RawImage _mouseRotateIndicator;

    [Header("Resize Tool")]
    public RectTransform ResizeBound;

    private void Awake()
    {
        UIManager.Instance.Register(this);
    }
}