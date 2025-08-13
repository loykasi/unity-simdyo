using TMPro;
using UnityEngine;

public class UIValueInputPort : UINodePort
{
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

        Vector2 size = _label.GetPreferredValues();
        _label.rectTransform.sizeDelta = new Vector2
        (
            size.x,
            _label.rectTransform.sizeDelta.y
        );
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