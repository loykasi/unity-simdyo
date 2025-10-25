using System.Collections.Generic;
using System.Linq;
using SFB;
using UnityEngine;

public class SaveLoadSystem : Singleton<SaveLoadSystem>
{
    [SerializeField] private DataService _dataService;
    private GameData _gameData = new();
    private List<ISaveable> _saveables;

    protected override void Awake()
    {
        base.Awake();
        _saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>().ToList();
        _saveables.Sort((s1, s2) => s1.SaveLoadOrder.CompareTo(s2.SaveLoadOrder));
    }

    public void Save()
    {
        _gameData.Clear();
        foreach (var item in _saveables)
        {
            item.SaveData(_gameData);
        }

        _dataService.Save(_gameData);
    }

    public void Load()
    {
        _dataService.Load(_gameData, OnSaveDataLoaded);
    }

    private void OnSaveDataLoaded()
    {
        foreach (var item in _saveables)
        {
            item.LoadData(_gameData);
        }
    }
}