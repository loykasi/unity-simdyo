using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Loykas.Scripting
{
    public class UILineConnection : MonoBehaviour, IGraphElement, IPointerEnterHandler, IPointerExitHandler
    {
        public NodeBoard Board { get; set; }
        public NodeConnection Connection { get; set; }
        public UILineRenderer LineRenderer;
        public UINodePort Source;
        public UINodePort Destination;

        public void Init(NodeBoard board, NodeConnection connection)
        {
            Board = board;
            Connection = connection;

            Connection.OnUpdated += OnUpdated;
        }

        private void OnDisable()
        {
            if (Connection != null)
            {
                Connection.OnUpdated -= OnUpdated;
            }
        }

        private void OnUpdated()
        {
            if (Connection.ShouldRemove)
            {
                Board.DeleteConnectionVisual(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LineRenderer.Thickness = 10;
            LineRenderer.UpdateVertex();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LineRenderer.Thickness = 6;
            LineRenderer.UpdateVertex();
        }

        public void Select()
        {
            LineRenderer.color = Color.blue;
            LineRenderer.UpdateVertex();
        }

        public void Delete()
        {
            Board.DeleteConnection(this);
        }

        public void DeleteVisual()
        {
            Source.DeleteConnection(this);
            Destination.DeleteConnection(this);
            Connection.OnUpdated -= OnUpdated;
            Destroy(gameObject);
        }

        public void Unselect()
        {
            LineRenderer.color = Color.white;
            LineRenderer.UpdateVertex();
        }
    }
}