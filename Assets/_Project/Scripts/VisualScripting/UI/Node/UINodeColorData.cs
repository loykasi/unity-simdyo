using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "NodeColorData", menuName = "Scriptable Objects/Node Color")]
    public class UINodeColorData : ScriptableObject
    {
        [System.Serializable]
        public struct UINodeColor
        {
            public ScriptNodeCategory Type;
            public Color Color;
        }

        public UINodeColor[] TypeColor;

        public Color Get(ScriptNodeCategory type)
        {
            foreach (var item in TypeColor)
            {
                if (item.Type == type)
                {
                    return item.Color;
                }
            }

            return default;
        }
    }
}