using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    void SetPath(string path);
    void Save(string name, GameData data);
    void Load(string name, GameData data);
}