using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocalizationToggle : MonoBehaviour
{
    public int ID;

    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _label;

    private UnityAction<int> _changeLanguageAction;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void Init(LocaleData data, UnityAction<int> callback)
    {
        _label.text = data.Name;
        ID = data.Index;

        _changeLanguageAction = callback;
    }

    public void SetStatus(bool isOn)
    {
        _toggle.SetIsOnWithoutNotify(isOn);
    }
    
    private void OnValueChanged(bool value)
    {
        _changeLanguageAction(ID);
    }
}