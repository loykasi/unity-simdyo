using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "Category", menuName = "Scriptable Objects/Visual Scripting/Category")]
    public class NodeCategoryData : ScriptableObject
    {
        public string Title;
    }
}