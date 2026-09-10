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
    [SerializeField] private GameObject _unknownState;

    private UnityAction<CollisionLayer, bool> _action;

    public void Init(CollisionLayer layer, UnityAction<CollisionLayer, bool> action)
    {
        _label.text = layer.ToString();
        Layer = layer;
        _action = action;
    }

    public void SetState(bool value)
    {
        _toggle.SetIsOnWithoutNotify(value);
        _unknownState.SetActive(false);
    }

    public void OnChanged(bool value)
    {
        _action?.Invoke(Layer, value);
        _unknownState.SetActive(false);
    }

    public void SetUnknownState()
    {
        _unknownState.SetActive(true);
        _toggle.SetIsOnWithoutNotify(false);
    }
}