using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationData", menuName = "Scriptable Objects/Localization Options Data")]
public class LocalizationDataSO : ScriptableObject
{
    public LocaleData[] Locales;
}