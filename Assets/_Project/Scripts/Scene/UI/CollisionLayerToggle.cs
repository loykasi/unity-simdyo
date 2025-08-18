using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollisionLayerToggle : MonoBehaviour
{
    public float Size => 30f;

    public CollisionLayer Layer;

    [SerializeField] private RectTransform _rect;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _label;

    private UnityAction<CollisionLayer, bool> _action;

    public void Init(Vector3 position, CollisionLayer layer, UnityAction<CollisionLayer, bool> action)
    {
        _rect.anchoredPosition = position;
        _label.text = layer.ToString();
        Layer = layer;
        _action = action;
    }

    public void SetState(bool value)
    {
        _toggle.SetIsOnWithoutNotify(value);
    }

    public void OnChanged(bool value)
    {
        _action?.Invoke(Layer, value);
    }
}