using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UITypeColorData _typeColorData;
    [SerializeField] private Image _portHandle;

    public void SetType(ScriptDataType type)
    {
        var display = _typeColorData.Get(type.Type);

        _portHandle.color = display.Color;
    }
}