using UnityEngine;

namespace Loykas.Scripting
{
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
        // private bool _hasPort = false;
        private NodePortEdge _edge;

        public void StartPreviewConnect(Vector3 startPosition, NodePortEdge edge)
        {
            _startPosition = startPosition - _previewLine.transform.position;
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
            Vector3 endPosition = mousePosition - _previewLine.transform.position;
            float dist = (mousePosition - _startPosition).sqrMagnitude;
            float delta = dist / (_maxDistance * _maxDistance);
            float radius = Mathf.Lerp(_cornerRadiusMinMax.x, _cornerRadiusMinMax.y, delta);

            _previewLine.CornerRadius = radius;

            switch (_edge)
            {
                case NodePortEdge.Left:
                    _previewLine.Points[0] = endPosition;
                    _previewLine.Points[1] = endPosition + Vector3.right * _offset;
                    break;
                case NodePortEdge.Right:
                    _previewLine.Points[2] = endPosition + Vector3.left * _offset;
                    _previewLine.Points[3] = endPosition;
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
            Vector3 localPosition = position - _connectedPreviewLine.transform.position;
            // _hasPort = true;
            _connectedPreviewLine.gameObject.SetActive(true);

            switch (_edge)
            {
                case NodePortEdge.Left:
                    _connectedPreviewLine.Points[0] = localPosition;
                    _connectedPreviewLine.Points[1] = localPosition + Vector3.right * _offset;
                    _connectedPreviewLine.Points[2] = _previewLine.Points[2];
                    _connectedPreviewLine.Points[3] = _previewLine.Points[3];
                    break;
                case NodePortEdge.Right:
                    _connectedPreviewLine.Points[0] = _previewLine.Points[0];
                    _connectedPreviewLine.Points[1] = _previewLine.Points[1];
                    _connectedPreviewLine.Points[2] = localPosition + Vector3.left * _offset;
                    _connectedPreviewLine.Points[3] = localPosition;
                    break;
            }
            _connectedPreviewLine.UpdateVertex();
        }

        public void ExitPort()
        {
            // _hasPort = false;
            _connectedPreviewLine.gameObject.SetActive(false);
        }
    }
}