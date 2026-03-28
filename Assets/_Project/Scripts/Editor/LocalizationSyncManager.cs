using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Reporting;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationSyncManager", menuName = "Localization/Localization Sync Manager")]
public class LocalizationSyncManager : ScriptableObject
{
    [SerializeField] private TextAsset _env;
    [SerializeField] private string _envPath;
    [SerializeField] private SheetsServiceProvider _sheetsServiceProvider;
    [SerializeField] private StringTableCollection[] _stringTables;

    public void Pull()
    {
        TemporaryEnvironment environment = EnvReader.Load(_envPath);
        string apiKey = environment.GetVariable("GOOGLE_SHEET_API_KEY");

        _sheetsServiceProvider.SetApiKey(apiKey);

        foreach (StringTableCollection table in _stringTables)
        {
            var extension = table.Extensions.FirstOrDefault(e => e is GoogleSheetsExtension) as GoogleSheetsExtension;
            PullExtension(extension);
        }

        _sheetsServiceProvider.SetApiKey(string.Empty);
    }

    static void PullExtension(GoogleSheetsExtension googleExtension)
    {
        // Setup the connection to Google
        GoogleSheets googleSheets = new(googleExtension.SheetsServiceProvider)
        {
            SpreadSheetId = googleExtension.SpreadsheetId
        };

        // Now update the collection. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
        googleSheets.PullIntoStringTableCollection
        (
            googleExtension.SheetId,
            googleExtension.TargetCollection as StringTableCollection,
            googleExtension.Columns,
            reporter: new ProgressBarReporter()
        );
    }
}

[CustomEditor(typeof(LocalizationSyncManager))]
public class LocalizationSyncManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        LocalizationSyncManager manager = (LocalizationSyncManager)target;
        if (GUILayout.Button("Pull", GUILayout.Height(30)))
        {
            manager.Pull();
        }
    }
}