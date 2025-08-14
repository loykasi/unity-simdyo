using TMPro;
using UnityEngine;

public class UIValueInputPort : UINodePort
{
    [SerializeField] private RectTransform _inputFieldRect;
    [SerializeField] private TMP_InputField _inputField;

    public override void Init()
    {
        base.Init();
        if (Port is InputValue valueInput)
        {
            if (valueInput.UseOptionalInput && _inputField != null)
            {
                _inputField.gameObject.SetActive(true);
                _inputField.text = valueInput.Value != null ? valueInput.Value.ToString() : "";
            }
            else
            {
                _inputField.gameObject.SetActive(false);
            }
        }

        UpdateSize();
    }

    private void UpdateSize()
    {
        Vector2 size = _label.GetPreferredValues();
        _label.rectTransform.sizeDelta = new Vector2
        (
            size.x,
            _label.rectTransform.sizeDelta.y
        );
        if (Port is InputValue valueInput)
        {
            if (valueInput.UseOptionalInput && _inputField != null)
            {
                _inputFieldRect.anchoredPosition = new Vector2(30f + size.x, 0f);
                Rect.sizeDelta = new Vector2
                (
                    30f + size.x + 50f,
                    30f
                );
            }
            else
            {
                Rect.sizeDelta = new Vector2
                (
                    30f + size.x,
                    30f
                );
            }
        }
    }

    public override void ValidConnection(IPort port)
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            if (_lineConnections[i].Source.Port != port)
            {
                _lineConnections[i].Delete();
            }
        }
    }
}