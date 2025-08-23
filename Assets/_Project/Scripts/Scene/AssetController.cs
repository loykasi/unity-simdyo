using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;

public class AssetController : Singleton<AssetController>
{
    private Dictionary<string, Texture2D> _textureTables = new();

    public bool TryChooseTextureFile(out Texture2D texture)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length == 0)
        {
            texture = null;
            return false;
        }

        string path = paths[0];

        if (_textureTables.TryGetValue(path, out texture))
        {
            return true;
        }

        byte[] data = File.ReadAllBytes(paths[0]);
        texture = new Texture2D(1024, 1024);
        texture.LoadImage(data);

        _textureTables.Add(path, texture);
        return true;
    }
}