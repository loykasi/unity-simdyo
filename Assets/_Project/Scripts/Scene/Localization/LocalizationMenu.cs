using System.Collections.Generic;
using UnityEngine;

public class LocalizationMenu : MonoBehaviour
{
    [SerializeField] private LocalizationDataSO _localizationData;
    [SerializeField] private LocalizationToggle _localizationTogglePrefab;
    [SerializeField] private Transform _holder;

    private LocalizationToggle _current;
    private List<LocalizationToggle> _toggles = new();

    private void Awake()
    {
        for (int i = 0; i < _localizationData.Locales.Length; i++)
        {
            LocaleData locale = _localizationData.Locales[i];

            LocalizationToggle toggle = Instantiate(_localizationTogglePrefab, _holder);
            toggle.transform.localPosition = new Vector3(0f, -30f * i, 0f);
            toggle.Init(locale, ChangeLanguage);

            _toggles.Add(toggle);
        }

        _current = _toggles[0];
        _current.SetStatus(true);
    }

    private void ChangeLanguage(int id)
    {
        if (_current.ID == id)
        {
            Close();
            return;
        }

        _current.SetStatus(false);
        _current = _toggles[id];
        _current.SetStatus(true);
        Close();

        GlobalLocalization.Instance.SetLanguage(id);
    }
    
    private void Close()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}