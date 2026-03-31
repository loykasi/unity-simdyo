using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RotateTool : PanTool
{
    public override ToolType Type => ToolType.Rotate;

    private bool _onRotation = false;
    private Vector3 _fromDirection;
    private float _startAngle;
    private SceneEntity _entity;
    private Quaternion _startQuaternion;

    [Header("Visualization")]
    [SerializeField] private float _snapRadius;
    [SerializeField] private RectTransform _visualization;

    [SerializeField] private Material _rotationMaterial;
    [SerializeField] private RawImage _mouseRotateIndicator;

    private Vector3 _startMouseDirection;

    private void Awake()
    {
        _mouseRotateIndicator.material = Instantiate(_rotationMaterial);
    }

    public override void OnUpdate()
    {
        Zoom();
        HandlePanRightMouse();
        HandleRotate();
        HandleSelection();
        HandleContextMenu();
    }

    private void HandleRotate()
    {
        Vector3 mousePosition = GetMouseWorldPositon();
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _entity = ObjectManager.Instance.SelectedObject;
            if (_entity == null)
            {
                return;
            }
            _fromDirection = mousePosition - _entity.transform.position;
            _startAngle = _entity.transform.eulerAngles.z;

            _onRotation = true;

            EnableVisualization(_entity.transform.position, mousePosition);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onRotation = false;
            DisableVisualization();
        }

        if (_onRotation)
        {
            Vector3 toDirection = mousePosition - _entity.transform.position;

            float angle = Vector3.SignedAngle(_fromDirection, toDirection, Vector3.forward);
            angle = _startAngle + angle;

            if (IsSnapping(mousePosition, _entity.transform.position))
            {
                angle = Mathf.Round(angle / 15f) * 15f;
            }
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            _entity.Rotation = rotation;

            UpdateUI(_entity.transform.position, mousePosition);
            
            Physics2D.SyncTransforms();

            // Debug.Log(angle);
            Debug.DrawRay(_entity.transform.position, _fromDirection, Color.red);
            Debug.DrawRay(_entity.transform.position, toDirection, Color.blue);
        }
    }

    public void EnableVisualization(Vector3 entityPosition, Vector3 mousePosition)
    {
        _startMouseDirection = mousePosition - entityPosition;
        float angle = Mathf.Atan2(_startMouseDirection.y, _startMouseDirection.x) * Mathf.Rad2Deg + 90f;
        _mouseRotateIndicator.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        _visualization.gameObject.SetActive(true);
    }

    public void DisableVisualization()
    {
        _visualization.gameObject.SetActive(false);
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
        return _snapRadius * scale;
    }

    public void UpdateUI(Vector3 worldPosition, Vector3 mousePosition)
    {
        UpdateMouseRotateIndicator(worldPosition, mousePosition);

        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);

        _visualization.position = screenPoint;
        _visualization.sizeDelta = _snapRadius * 2f * Vector2.one;
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

        _mouseRotateIndicator.material.SetFloat("_Percentage", Mathf.Abs(angle / 360f));
        _mouseRotateIndicator.material.SetFloat("_Flip", angle < 0 ? 1 : -1);

        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);
        Vector3 mouseScreenPoint = camera.WorldToScreenPoint(mousePosition);
        float size = Vector3.Distance(mouseScreenPoint, screenPoint) * 2f / EngineManager.Instance.CanvasScale;

        _mouseRotateIndicator.rectTransform.sizeDelta = new Vector2(size, size);
    }
}