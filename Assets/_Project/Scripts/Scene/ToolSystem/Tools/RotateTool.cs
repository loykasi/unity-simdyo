using System.Collections.Generic;
using UnityEngine;

public class RotateTool : BaseTool
{
    public override ToolType Type => ToolType.Rotate;

    private bool _onRotation = false;
    private Vector3 _fromDirection;
    private List<float> _angles = new();
    private List<Vector3> _directions = new();
    private Vector3 _startMouseDirection;
    private bool _isFirstRun = true;

    private UIToolManager _uiToolManager;
    private EntityGroup _selectionGroup => ObjectManager.Instance.SelectionGroup;

    public override void Enable()
    {
        base.Enable();

        if (_uiToolManager == null)
        {
            _uiToolManager = UIManager.Instance.Get<UIToolManager>();
        }
    }

    protected override void OnClick()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        if (_selectionGroup.Count == 0)
        {
            return;
        }

        _onRotation = true;
        _isFirstRun = false;
    }

    protected override void OnClickReleased()
    {
        if (_onRotation)
        {
            _onRotation = false;
            DisableVisualization();
        }
    }

    protected override void OnPointMove(Vector2 value)
    {
        base.OnPointMove(value);

        if (_onRotation)
        {
            Vector3 mouseWorldPosition = Utils.ToWorldPositon(value);
            List<SceneEntity> entites = _selectionGroup.Entities;
            
            if (!_isFirstRun)
            {
                int count = entites.Count;
                _angles.Clear();
                _directions.Clear();
                _fromDirection = mouseWorldPosition - _selectionGroup.Center;
                for (int i = 0; i < count; i++)
                {
                    _directions.Add(entites[i].Position - _selectionGroup.Center);
                    _angles.Add(entites[i].Angle);
                }
                
                EnableVisualization(_selectionGroup.Center, mouseWorldPosition);
                _isFirstRun = true;
            }

            Vector3 toDirection = mouseWorldPosition - _selectionGroup.Center;

            float angle = Vector3.SignedAngle(_fromDirection, toDirection, Vector3.forward);

            if (IsSnapping(mouseWorldPosition, _selectionGroup.Center))
            {
                angle = Mathf.Round(angle / 15f) * 15f;
            }

            for (int i = 0; i < entites.Count; i++)
            {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _directions[i];

                entites[i].Position = _selectionGroup.Center + direction;
                entites[i].Angle = _angles[i] + angle;
            }

            UpdateUI(_selectionGroup.Center, mouseWorldPosition);
            
            Physics2D.SyncTransforms();
            Debug.DrawRay(_selectionGroup.Center, _fromDirection, Color.red);
            Debug.DrawRay(_selectionGroup.Center, toDirection, Color.blue);
        }
    }

    public void EnableVisualization(Vector3 entityPosition, Vector3 mousePosition)
    {
        _startMouseDirection = mousePosition - entityPosition;
        float angle = Mathf.Atan2(_startMouseDirection.y, _startMouseDirection.x) * Mathf.Rad2Deg + 90f;
        
        _uiToolManager._mouseRotateIndicator.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        _uiToolManager._visualization.gameObject.SetActive(true);
        UpdateUI(_selectionGroup.Center, mousePosition);
    }

    public void DisableVisualization()
    {
        _uiToolManager._visualization.gameObject.SetActive(false);
    }

    public bool IsSnapping(Vector3 mousePosition, Vector3 entityPosition)
    {
        float centerToMouseSqrDist = (mousePosition - entityPosition).sqrMagnitude;
        float radius = GetSnapRadiusWorld();
        return centerToMouseSqrDist < radius * radius;
    }

    public float GetSnapRadiusWorld()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        float scale = camera.orthographicSize * 2 / Screen.height * EngineManager.Instance.CanvasScale;
        return ToolManager.Instance.SnapRadius * scale;
    }

    public void UpdateUI(Vector3 worldPosition, Vector3 mousePosition)
    {
        UpdateMouseRotateIndicator(worldPosition, mousePosition);

        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);

        _uiToolManager._visualization.position = screenPoint;
        _uiToolManager._visualization.sizeDelta = ToolManager.Instance.SnapRadius * 2f * Vector2.one;
    }
    
    private void UpdateMouseRotateIndicator(Vector3 worldPosition, Vector3 mousePosition)
    {
        Vector3 toDirection = mousePosition - worldPosition;
        float angle = Vector3.SignedAngle(_startMouseDirection, toDirection, Vector3.forward);

        if (IsSnapping(mousePosition, worldPosition))
        {
            angle = Mathf.Round(angle / 15f) * 15f;
        }

        // _mouseRotateImage.fillClockwise = angle < 0;
        // _mouseRotateImage.fillAmount = Mathf.Abs(angle / 360f);

        _uiToolManager._mouseRotateIndicator.material.SetFloat("_Percentage", Mathf.Abs(angle / 360f));
        _uiToolManager._mouseRotateIndicator.material.SetFloat("_Flip", angle < 0 ? 1 : -1);

        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);
        Vector3 mouseScreenPoint = camera.WorldToScreenPoint(mousePosition);
        float size = Vector3.Distance(mouseScreenPoint, screenPoint) * 2f / EngineManager.Instance.CanvasScale;

        _uiToolManager._mouseRotateIndicator.rectTransform.sizeDelta = new Vector2(size, size);
    }
}