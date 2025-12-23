using UnityEngine;

public class TextureData
{
    public string Key;
    public Texture2D Texture;

    public TextureData(string key, Texture2D texture)
    {
        Key = key;
        Texture = texture;
    }
}