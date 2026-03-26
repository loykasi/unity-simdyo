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
            Exit = CreateOutputTrigger(nameof(Exit));

            List = CreateInputValue
            (
                nameof(List),
                ScriptDataType.List(DataType.Any),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );

            IndexA = CreateInputValue
            (
                nameof(IndexA),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );

            IndexB = CreateInputValue
            (
                nameof(IndexB),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );
        }

        public OutputTrigger Swap(NodeTask task)
        {
            IList list = List.GetValue().ListValue;

            int indexA = Utils.ObjectToIndex(IndexA.GetValue());
            int indexB = Utils.ObjectToIndex(IndexB.GetValue());

            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
            return Exit;
        }
    }
}