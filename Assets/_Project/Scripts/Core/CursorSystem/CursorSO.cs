using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursor", menuName = "Scriptable Objects/Cursor")]
public class CursorSO : ScriptableObject
{
    [System.Serializable]
    public class CursorData
    {
        public CursorType Type;
        public Texture2D Texture;
    }

    public Texture2D Default;
    public CursorData[] Cursors;
    public Dictionary<CursorType, Texture2D> CursorTable = new();

    private void OnValidate()
    {
        CursorTable.Clear();
        foreach (var cursor in Cursors)
        {
            CursorTable.Add(cursor.Type, cursor.Texture);
        }
    }

    public Texture2D Get(CursorType type)
    {
        if (CursorTable.TryGetValue(type, out Texture2D cursor))
        {
            return cursor;
        }

        return Default;
    }
}