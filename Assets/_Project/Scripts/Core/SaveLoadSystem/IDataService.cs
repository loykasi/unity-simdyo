using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    void Save(string name, GameData data);
    void Load(string name, GameData data);
}