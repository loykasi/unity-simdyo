using System;
using UnityEngine;

public class ResizeController : Singleton<ResizeController>
{
    [SerializeField] private RectTransform _bound;

    private ResizeBox _resizeBox = new();

    private IResize _handler;

    private void Start()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnObjectDeselected()
    {
        _bound.gameObject.SetActive(false);
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (entity is BoxEntity boxEntity)
        {
            _bound.gameObject.SetActive(true);

            boxEntity.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            Vector2 size = new(boxEntity.Width, boxEntity.Height);

            Camera camera = EngineManager.Instance.EditorCamera;
            float scale = Screen.height / (camera.orthographicSize * 2);

            _bound.position = camera.WorldToScreenPoint(position);
            _bound.rotation = rotation;
            _bound.sizeDelta = size * scale;

            _handler = _resizeBox;
            _handler.Init(boxEntity);
        }
    }

    private void Update()
    {
        if (ObjectManager.Instance.SelectedObject != null)
        {
            if (ObjectManager.Instance.SelectedObject is BoxEntity boxEntity)
            {
                _bound.gameObject.SetActive(true);

                boxEntity.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

                Vector2 size = new(boxEntity.Width, boxEntity.Height);
                Camera camera = EngineManager.Instance.EditorCamera;
                float scale = Screen.height / (camera.orthographicSize * 2);

                _bound.position = camera.WorldToScreenPoint(position);
                _bound.rotation = rotation;
                _bound.sizeDelta = size * scale;
            }
        }
    }

    public void BeginResize(BoundsHandleDirection direction)
    {
        _handler.BeginResize(direction);
    }

    public void Resize(BoundsHandleDirection direction, Vector3 mousePosition)
    {
        _handler.Resize(direction, mousePosition);
    }
}