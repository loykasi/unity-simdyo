using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "NodeCollectionData", menuName = "Scriptable Objects/Visual Scripting/Node Collection")]
    public class NodeCollectionData : ScriptableObject
    {
        public ScriptNodeData[] Nodes;
    }
}