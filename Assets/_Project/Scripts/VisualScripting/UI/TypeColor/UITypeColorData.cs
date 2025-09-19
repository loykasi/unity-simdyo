using UnityEngine;

[CreateAssetMenu(fileName = "TypeColorData", menuName = "Scriptable Objects/Type Color")]
public class UITypeColorData : ScriptableObject
{
    [System.Serializable]
    public struct UITypeColor
    {
        public DataType Type;
        public Color Color;
        public string DisplayName;
    }

    public UITypeColor[] TypeColor;

    public UITypeColor Get(DataType type)
    {
        foreach (var item in TypeColor)
        {
            if (item.Type == type)
            {
                return item;
            }
        }

        return default;
    }
}