using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class GlobalLocalization : Singleton<GlobalLocalization>
{
    public StringTable VisualScriptingTable;

    // IEnumerator Start()
    // {
    //     yield return LocalizationSettings.InitializationOperation;
    // }

    public void SetLanguage(int id)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
    }

    public string GetValue(string key)
    {
        VisualScriptingTable = LocalizationSettings.StringDatabase.GetTable("VisualScripting");
        StringTableEntry entry = VisualScriptingTable.GetEntry(key);
        return entry != null ? entry.LocalizedValue : key;

        // return LocalizationSettings.StringDatabase.GetLocalizedString("VisualScripting", key);
    }
}