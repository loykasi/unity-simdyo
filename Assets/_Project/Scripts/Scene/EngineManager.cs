using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public GameObject SelectedObject;

    [SerializeField] private float _selectRadius;

    public void Select(Vector3 worldPoint)
    {
        worldPoint.z = 0;

        Collider2D collider = Physics2D.OverlapCircle(worldPoint, _selectRadius);

        if (collider == null)
        {
            SelectedObject = null;
            return;
        }

        Debug.Log(collider);
        SelectedObject = collider.gameObject;

        VisualScripting vs = SelectedObject.GetComponent<VisualScripting>();
        NodeBoard.Instance.SetVisualScripting(vs);
    }

    private void OnGUI()
    {
        if (SelectedObject != null)
        {
            GUILayout.Label(SelectedObject.name);
        }
    }
}