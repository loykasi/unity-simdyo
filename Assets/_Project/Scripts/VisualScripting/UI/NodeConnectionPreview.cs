using UnityEngine;

public class NodeConnectionPreview : MonoBehaviour
{
    public UILineRenderer LineRenderer
    {
        get => _connectedPreviewLine;
    }

    [SerializeField] private UILineRenderer _previewLine;
    [SerializeField] private UILineRenderer _connectedPreviewLine;
    [SerializeField] private float _offset;
    [SerializeField] private Vector2 _cornerRadiusMinMax;
    [SerializeField] private float _maxDistance;

    private Vector3 _startPosition;
    private bool _hasPort = false;
    private NodePortEdge _edge;

    public void StartPreviewConnect(Vector3 startPosition, NodePortEdge edge)
    {
        _startPosition = startPosition;
        _edge = edge;
        _previewLine.gameObject.SetActive(true);

        switch (_edge)
        {
            case NodePortEdge.Left:
                _previewLine.Points[2] = _startPosition + Vector3.left * _offset;
                _previewLine.Points[3] = _startPosition;
                break;
            case NodePortEdge.Right:
                _previewLine.Points[0] = _startPosition;
                _previewLine.Points[1] = _startPosition + Vector3.right * _offset;
                break;
        }
    }

    public void DragPreviewConnect(Vector3 mousePosition)
    {
        float dist = (mousePosition - _startPosition).sqrMagnitude;
        float delta = dist / (_maxDistance * _maxDistance);
        float radius = Mathf.Lerp(_cornerRadiusMinMax.x, _cornerRadiusMinMax.y, delta);

        _previewLine.CornerRadius = radius;

        switch (_edge)
        {
            case NodePortEdge.Left:
                _previewLine.Points[0] = mousePosition;
                _previewLine.Points[1] = mousePosition + Vector3.right * _offset;
                break;
            case NodePortEdge.Right:
                _previewLine.Points[2] = mousePosition + Vector3.left * _offset;
                _previewLine.Points[3] = mousePosition;
                break;
        }
        _previewLine.UpdateVertex();
    }

    public void EndPreviewConnect()
    {
        _previewLine.gameObject.SetActive(false);
        _connectedPreviewLine.gameObject.SetActive(false);
    }

    public void EnterPort(Vector3 position)
    {
        _hasPort = true;
        _connectedPreviewLine.gameObject.SetActive(true);

        switch (_edge)
        {
            case NodePortEdge.Left:
                _connectedPreviewLine.Points[0] = position;
                _connectedPreviewLine.Points[1] = position + Vector3.right * _offset;
                _connectedPreviewLine.Points[2] = _previewLine.Points[2];
                _connectedPreviewLine.Points[3] = _previewLine.Points[3];
                break;
            case NodePortEdge.Right:
                _connectedPreviewLine.Points[0] = _previewLine.Points[0];
                _connectedPreviewLine.Points[1] = _previewLine.Points[1];
                _connectedPreviewLine.Points[2] = position + Vector3.left * _offset;
                _connectedPreviewLine.Points[3] = position;
                break;
        }
        _connectedPreviewLine.UpdateVertex();
    }

    public void ExitPort()
    {
        _hasPort = false;
        _connectedPreviewLine.gameObject.SetActive(false);
    }
}