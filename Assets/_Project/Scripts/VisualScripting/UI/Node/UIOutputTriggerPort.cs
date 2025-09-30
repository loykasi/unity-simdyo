using UnityEngine;

namespace Loykas.Scripting
{
    public class UIOutputTriggerPort : UINodePort
    {
        public override NodePortEdge Edge => NodePortEdge.Right;
        private readonly float _handleSize = 20f;
        private readonly float _height = 30f;

        public override void Init()
        {
            base.Init();

            UpdateSize();
        }

        private void UpdateSize()
        {
            UpdateLabel();

            float width = _handleSize;

            if (Port.ShouldShowLabel)
            {
                width += _label.rectTransform.sizeDelta.x;
            }

            Rect.sizeDelta = new Vector2(width, _height);
        }
    }
}
