using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TextBoxSizeFilter : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private LocalizeStringEvent _localizeStringEvent;
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private float _totalPaddingX;

    private void OnEnable()
    {
        _localizeStringEvent.OnUpdateString.AddListener(OnUpdateString);
    }

    private void OnDisable()
    {
        _localizeStringEvent.OnUpdateString.RemoveListener(OnUpdateString);
    }

    private void OnUpdateString(string value)
    {
        Vector2 preferredSize = _textField.GetPreferredValues(value);
        _rect.sizeDelta = new(preferredSize.x + _totalPaddingX, _rect.sizeDelta.y);
    }
}