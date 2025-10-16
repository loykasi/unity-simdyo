using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public ColorHSV BackgroundColor;
    public Vector3 CameraPosition;
    public float CameraSize;
    public ScriptFlowData GlobalScript = new();

    public List<EntityData> Entities = new();

    public void Clear()
    {
        GlobalScript.Clear();
        Entities.Clear();
    }
}