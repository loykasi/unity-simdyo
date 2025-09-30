using UnityEngine;

namespace Loykas.Scripting
{
    public abstract class ScriptNodeData : ScriptableObject
    {
        public string Title;
        public NodeCategoryData Category;

        public abstract ScriptNode Create();
    }
}