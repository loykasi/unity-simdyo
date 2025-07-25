using TMPro;
using UnityEngine;

public class UIValueInputPort : UINodePort
{
    [SerializeField] private TMP_InputField _inputField;
    
    public override void Init()
    {
        if (Port is ValueInput valueInput)
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