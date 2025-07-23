using UnityEngine;

public class NodeButtons : MonoBehaviour
{
    // public VisualScripting VisualScripting;
    [SerializeField] private NodeCollectionData _nodeCollection;

    private void OnGUI()
    {
        for (int i = 0; i < _nodeCollection.Nodes.Length; i++)
        {
            string title = _nodeCollection.Nodes[i].Title;
            if (GUI.Button(new Rect(10, 10 + 70 * i, 100, 50), title))
            {
                // Debug.Log($"Add \"{title}\" node");
                NodeBoard.Instance.AddNode(_nodeCollection.Nodes[i]);
            }
        }
    }
}