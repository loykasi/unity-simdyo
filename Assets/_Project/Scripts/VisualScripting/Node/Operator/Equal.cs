using System;
using UnityEngine;

namespace Loykas.Scripting
{
    class EqualNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new EqualNode();
        }

        public EqualNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Any))
                .NoLocalize();

            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Any))
                .NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get).HideLabel();

            A.OnConnected += OnAConnected;
            B.OnConnected += OnBConnected;
        }

        private object Get()
        {
            object a = A.GetValue();
            object b = B.GetValue();
            
            if (a.GetType() == b.GetType())
            {
                return a == b;
            }

            return false;
        }

        private void OnAConnected()
        {
            // if (A.Source != null)
            // {
            //     ScriptDataType type = A.Source.Type;
            //     B.SetType(type);
            // }
            // else
            // {
            //     A.SetType(ScriptDataType.Single(DataType.Any));
            // }

            // OnNodeUpdated?.Invoke();
        }

        private void OnBConnected()
        {
            
        }
    }
}