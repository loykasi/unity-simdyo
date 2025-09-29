using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "CategoryCollection", menuName = "Scriptable Objects/Visual Scripting/Category Collection")]
    public class NodeCategoryCollection : ScriptableObject
    {
        public NodeCategoryData[] Categories;
    }
}