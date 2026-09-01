using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMove
{
    private bool _onMovingObject;
    private bool _isInitialized;
    private Vector3 _mouseStartPosition;
    private readonly float[] _asixPoints = new float[3];
    private readonly List<Vector3> _startPositions = new();

    private readonly Action _clickEvent;
    private readonly Action _clickReleasedEvent;
    private readonly Action<Vector2> _mouseMoveEvent;

    private EntityGroup _selectionGroup => ObjectManager.Instance.SelectionGroup;

    public InteractionMove()
    {
        _clickEvent = OnClick;
        _clickReleasedEvent = OnClickReleased;
        _mouseMoveEvent = OnPointMove;
    }

    public void Enable()
    {
        InputManager.Instance.ClickEvent += _clickEvent;
        InputManager.Instance.ClickReleasedEvent += _clickReleasedEvent;
        InputManager.Instance.MouseMoveEvent += _mouseMoveEvent;
    }

    public void Disable()
    {
        InputManager.Instance.ClickEvent -= _clickEvent;
        InputManager.Instance.ClickReleasedEvent -= _clickReleasedEvent;
        InputManager.Instance.MouseMoveEvent -= _mouseMoveEvent;
    }

    private void OnClick()
    {
        if (ScreenInteractionUtils.IsOverUI()
            || _selectionGroup.Count == 0)
        {
            return;
        }

        Vector2 mousePosition = InputManager.Instance.MousePosition;

        if (Utils.IsMouseOverSelections(mousePosition))
        {
            _onMovingObject = true;
            _isInitialized = false;
        }
    }

    private void OnClickReleased()
    {
        _onMovingObject = false;

        foreach (SceneEntity entity in _selectionGroup.Entities)
        {
            if (entity is TracerEntity tracerEntity)
            {
                tracerEntity.AutoAttachToMeshEntity();
            }    
        }

        Physics2D.SyncTransforms();
    }

    private void OnPointMove(Vector2 value)
    {
        if (_onMovingObject)
        {
            if (!_isInitialized)
            {
                _mouseStartPosition = Utils.ToWorldPositon(value);

                _startPositions.Clear();
                foreach (var entity in _selectionGroup.Entities)
                {
                    _startPositions.Add(entity.Position);
                }
                _isInitialized = true;
            }
            ApplyMovementWithSnapping(value);
        }
    }

    private void ApplyMovementWithSnapping(Vector3 mousePosition)
    {
        Vector3 center = _selectionGroup.Bounds.center;
        Vector3 extents = _selectionGroup.Bounds.extents;

        Vector3 mouseOffset = Utils.ToWorldPositon(mousePosition) - _mouseStartPosition;

        float snappedX = GetSnappedOffset(mouseOffset, center, extents.x, isYAsis: false);
        float snappedY = GetSnappedOffset(mouseOffset, center, extents.y, isYAsis: true);

        for (int i = 0; i < _selectionGroup.Entities.Count; i++)
        {
            var entity = _selectionGroup.Entities[i];
            entity.Position = _startPositions[i] + new Vector3(snappedX, snappedY, 0);      
        }
        _selectionGroup.Update();
    }

    private float GetSnappedOffset(Vector3 mouseOffset, Vector3 center, float extent, bool isYAsis)
    {
        _asixPoints[0] = 0;
        _asixPoints[1] = extent;
        _asixPoints[2] = - extent;
        
        float bestOffset = 0;
        float minSqrLen = float.MaxValue;

        foreach (float axisPoint in _asixPoints)
        {
            Vector3 point = center + (isYAsis ? new Vector3(0, axisPoint, 0) : new Vector3(axisPoint, 0, 0));
            CalculateSnapping(point, mouseOffset, isYAsis: isYAsis, out float offset, out float sqrLen);

            if (sqrLen < minSqrLen)
            {
                minSqrLen = sqrLen;
                bestOffset = offset;
            }
        }

        return bestOffset;
    }

    private void CalculateSnapping(Vector3 point, Vector3 mouseOffset, bool isYAsis, out float moveOffset, out float minSqrLen)
    {
        Vector3 movePoint = point + mouseOffset;
        Vector3 gridPos = Vector3Utils.GetGridPosition(movePoint);
        
        moveOffset = isYAsis ? (gridPos.y - point.y) : (gridPos.x - point.x);
        minSqrLen = Mathf.Abs(isYAsis ? (gridPos.y - movePoint.y) : (gridPos.x - movePoint.x));
    }
}