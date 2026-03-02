using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EntityContextMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvas;
    private bool _isHover = false;

    private SceneEntity _entity;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
        {
            Close();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
    }

    public void Open(SceneEntity entity)
    {
        _entity = entity;
        gameObject.SetActive(true);
        
        Vector3 worldPosition = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, worldPosition, null, out Vector2 point);
        Vector2 position = point + new Vector2(_canvas.sizeDelta.x * 0.5f, _canvas.sizeDelta.y * 0.5f);
        
        float xDiff = position.x + _rect.sizeDelta.x - _canvas.sizeDelta.x;
        float yDiff = _rect.sizeDelta.y - position.y;

        if (xDiff > 0)
        {
            position.x -= xDiff;
        }

        if (yDiff > 0)
        {
            position.y += yDiff;
        }

        _rect.anchoredPosition = position;
    }

    private void Close()
    {
        _entity = null;
        gameObject.SetActive(false);
    }

    public void Clone()
    {
        SceneEntity entity = _entity.CloneEntity();

        Vector3 position = entity.Position + new Vector3(entity.Bounds.size.x, 0f, 0f);
        entity.Position = position;

        Physics2D.SyncTransforms();
    }

    public void Delete()
    {
        ObjectManager.Instance.DeleteEntity(_entity);
        Close();
    }

    public void Intersect()
    {
        ObjectManager.Instance.DoIntersection(_entity);
        Close();
    }

    public void Subtract()
    {
        ObjectManager.Instance.DoSubtract(_entity);
        Close();
    }

    public void MoveToBack()
    {
        ObjectManager.Instance.MoveToBack(_entity);
        Close();
    }

    public void MoveToFront()
    {
        ObjectManager.Instance.MoveToFront(_entity);
        Close();
    }

    public void ResizeByTexture()
    {
        if (_entity.EntityType != EntityType.Box)
        {
            return;
        }

        var boxEntity = (BoxEntity)_entity;
        boxEntity.ResizeByTexture();

        Physics2D.SyncTransforms();
    }
}