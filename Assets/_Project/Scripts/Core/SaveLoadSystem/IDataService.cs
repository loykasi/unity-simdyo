using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDataService
{
    void Save(GameData data);
    void Load(GameData data, UnityAction onSuccess, UnityAction onFailure);
}