using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public SceneEntity SelectedObject { get; set; }

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
                SelectedObject.gameObject.layer = _defaultLayer;
                SelectedObject = null;
            }
            return;
        }

        if (SelectedObject != null)
        {
            SelectedObject.gameObject.layer = _defaultLayer;
        }

        SelectedObject = collider.GetComponent<SceneEntity>();
        SelectedObject.gameObject.layer = _selectLayer;
    }
}