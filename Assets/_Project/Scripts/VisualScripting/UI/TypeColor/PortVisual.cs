using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UITypeColorData _typeColorData;

    [SerializeField] private RawImage _portHandle;

    [Header("Single")]
    [SerializeField] private GameObject _singleContainer;
    [SerializeField] private TextMeshProUGUI _singleTypeLabel;

    [Header("List")]
    [SerializeField] private GameObject _listContainer;
    [SerializeField] private TextMeshProUGUI _listKindLabel;
    [SerializeField] private TextMeshProUGUI _listTypeLabel;

    public void SetType(ScriptDataType type)
    {
        var display = _typeColorData.Get(type.Type);

        _portHandle.color = display.Color;

        if (type.IsList)
        {
            SetupListDisplay(display);
        }
        else
        {
            SetupSingleDisplay(display);
        }
    }

    private void SetupSingleDisplay(UITypeColorData.UITypeColor display)
    {
        _singleContainer.SetActive(true);
        _listContainer.SetActive(false);

        _singleTypeLabel.color = display.Color;
        _singleTypeLabel.text = display.DisplayName;
    }

    private void SetupListDisplay(UITypeColorData.UITypeColor display)
    {
        _singleContainer.SetActive(false);
        _listContainer.SetActive(true);

        _listKindLabel.color = display.Color;
        _listKindLabel.text = "List";

        _listTypeLabel.color = display.Color;
        _listTypeLabel.text = display.DisplayName;
    }
}