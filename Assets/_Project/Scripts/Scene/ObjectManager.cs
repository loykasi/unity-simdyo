using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectManager : Singleton<ObjectManager>
{
    public event UnityAction<SceneEntity> OnObjectSelected;
    public event UnityAction OnObjectDeselected;

    public List<SceneEntity> SceneEntities = new();
    public SceneEntity SelectedObject { get; set; }

    [SerializeField] private float _selectRadius;
    [SerializeField] private int _defaultLayer;
    [SerializeField] private int _selectLayer;

    public void Select(Vector3 screenPoint)
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Ray ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Vector3 worldPoint = camera.ScreenToWorldPoint(screenPoint);

        // worldPoint.z = 0;
        // Collider2D collider = Physics2D.OverlapCircle(worldPoint, _selectRadius);

        if (hit.collider == null)
        {
            if (SelectedObject != null)
            {
                SelectedObject.gameObject.layer = _defaultLayer;
                SelectedObject = null;
            }

            OnObjectDeselected?.Invoke();
            return;
        }

        if (SelectedObject != null)
        {
            SelectedObject.gameObject.layer = _defaultLayer;
        }

        SelectedObject = hit.collider.GetComponent<SceneEntity>();
        SelectedObject.gameObject.layer = _selectLayer;

        OnObjectSelected?.Invoke(SelectedObject);
    }

    public void AddEntity(SceneEntity entity)
    {
        SceneEntities.Add(entity);
    }
}