using System.Collections.Generic;
using System.Linq;
using SFB;
using UnityEngine;

public class SaveLoadSystem : Singleton<SaveLoadSystem>
{
    public string Name = "GameScene";
    private GameData _gameData = new();
    private IDataService _dataService = new DataService(new JsonSerializer());
    private List<ISaveable> _saveables;

    protected override void Awake()
    {
        base.Awake();
        _saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToList();
        _saveables.Sort((s1, s2) => s1.SaveLoadOrder.CompareTo(s2.SaveLoadOrder));
    }

    public void SaveScene()
    {
        _gameData.Clear();
        foreach (var item in _saveables)
        {
            item.SaveData(_gameData);
        }

        _dataService.Save(Name, _gameData);
    }

    public void Save()
    {
        // string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "zip");
        StandaloneFileBrowser.SaveFilePanelAsync("Save File", "", "", "zip", Save);   
    }
    
    public void Save(string path)
    {
        Debug.Log($"Save at: {path}");
        _dataService.SetPath(path);

        _gameData.Clear();
        foreach (var item in _saveables)
        {
            item.SaveData(_gameData);
        }

        _dataService.Save(Name, _gameData);
    }

    public void Load()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length == 0)
        {
            return;
        }

        string path = paths[0];

        _dataService.SetPath(path);
        _dataService.Load(Name, _gameData);

        foreach (var item in _saveables)
        {
            item.LoadData(_gameData);
        }
    }

    public void LoadScene()
    {
        _dataService.Load(Name, _gameData);

        foreach (var item in _saveables)
        {
            item.LoadData(_gameData);
        }
    }
}