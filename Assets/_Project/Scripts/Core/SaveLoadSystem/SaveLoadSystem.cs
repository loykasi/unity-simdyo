using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadSystem : Singleton<SaveLoadSystem>
{
    public GameData GameData = new();
    private IDataService _dataService = new DataService(new JsonSerializer());
    private List<ISaveable> _saveables;

    protected override void Awake()
    {
        base.Awake();
        _saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToList();
    }

    public void SaveScene()
    {
        foreach (var item in _saveables)
        {
            item.SaveData(GameData);
        }
        _dataService.Save(GameData);
    }

    public void LoadScene()
    {
        string name = GameData.Name;
        GameData = _dataService.Load(name);

        foreach (var item in _saveables)
        {
            item.LoadData(GameData);
        }
    }
}