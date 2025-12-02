using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameData
{
    public SceneData Scene = new();

    [JsonIgnore]
    public List<Texture2D> Textures;

    public void Clear()
    {
        Scene.Clear();
    }
}