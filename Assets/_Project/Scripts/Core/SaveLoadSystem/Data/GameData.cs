using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameData
{
    public SceneData Scene = new();

    [JsonIgnore]
    public List<TextureData> Textures = new();

    public void Clear()
    {
        Scene.Clear();
    }
}