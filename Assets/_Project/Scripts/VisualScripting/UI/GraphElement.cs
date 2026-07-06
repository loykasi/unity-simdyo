using UnityEngine;

namespace Loykas.Scripting
{
    public interface IGraphElement
    {
        void Select();
        void Unselect();
        void Delete();
        void BeginMove();
        void Move(Vector2 delta);
    }
}