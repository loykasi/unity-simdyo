using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class SwapValuesNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue List;
        public InputValue IndexA;
        public InputValue IndexB;

        public override ScriptNode Create()
        {
            return new SwapValuesNode();
        }

        public SwapValuesNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Swap);
            Exit = OutputTrigger(nameof(Exit));

            List = InputValue(nameof(List), ScriptDataType.List(DataType.Any)).NoLocalize();
            IndexA = InputValue(nameof(IndexA), ScriptDataType.Single(DataType.Number)).NoLocalize();
            IndexB = InputValue(nameof(IndexB), ScriptDataType.Single(DataType.Number)).NoLocalize();
        }

        public OutputTrigger Swap(NodeTask task)
        {
            IList list = (IList)List.GetValue();

            int indexA = Utils.ObjectToIndex(IndexA.GetValue());
            int indexB = Utils.ObjectToIndex(IndexB.GetValue());

            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
            return Exit;
        }
    }
}