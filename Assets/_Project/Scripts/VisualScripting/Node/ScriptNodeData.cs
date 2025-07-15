using UnityEngine;

public abstract class ScriptNodeData : ScriptableObject
{
    public string Title;
    public abstract ScriptNode Create();

    private void Awake()
    {
        Title = GetType().Name;
    }
}