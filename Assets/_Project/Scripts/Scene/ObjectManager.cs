using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public GameObject SelectedObject { get; set; }

    [SerializeField] private float _selectRadius;
    [SerializeField] private int _defaultLayer;
    [SerializeField] private int _selectLayer;

    public void Select(Vector3 worldPoint)
    {
        worldPoint.z = 0;

        Collider2D collider = Physics2D.OverlapCircle(worldPoint, _selectRadius);

        if (collider == null)
        {
            if (SelectedObject != null)
            {
                SelectedObject.layer = _defaultLayer;
                SelectedObject = null;
            }
            return;
        }

        if (SelectedObject != null)
        {
            SelectedObject.layer = _defaultLayer;
        }

        SelectedObject = collider.gameObject;
        SelectedObject.layer = _selectLayer;

        VisualScripting vs = SelectedObject.GetComponent<VisualScripting>();
        NodeBoard.Instance.SetVisualScripting(vs);
    }
}