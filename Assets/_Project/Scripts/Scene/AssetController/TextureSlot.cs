using UnityEngine;
using UnityEngine.Events;

public class TextureSlot
{
    public event UnityAction OnRemoved;
    public Texture2D Texture;

    public void Remove()
    {
        OnRemoved?.Invoke();
    }
}