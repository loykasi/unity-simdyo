using UnityEngine;

namespace Loykas.Scripting
{
    class ForNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;
        public OutputTrigger Completed;
        public OutputTrigger LoopBody;

        public InputValue FirstIndex;
        public InputValue LastIndex;
        public InputValue Step;
        public OutputValue Index;

        private int _index;
        private bool _firstRun = true;

        public ForNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Loop);
            
            Completed = CreateOutputTrigger
            (
                nameof(Completed),
                PortSettings.Default
            );
            
            LoopBody = CreateOutputTrigger
            (
                nameof(LoopBody),
                PortSettings.Default
            );

            FirstIndex = CreateInputValue
            (
                nameof(FirstIndex),
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            ).UseInput();

            LastIndex = CreateInputValue
            (
                nameof(LastIndex),
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            ).UseInput();

            Step = CreateInputValue
            (
                nameof(Step),
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            ).UseInput();

            Index = CreateOutputValue
            (
                nameof(Index),
                GetIndex,
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            );
        }

        public override ScriptNode Create()
        {
            return new ForNode();
        }

        public override void Reset()
        {
            Debug.Log("reset");
            _firstRun = true;
        }

        private ValueTransfer GetIndex()
        {
            return ValueTransfer.CreateNumber(_index);
        }

        private OutputTrigger Loop(NodeTask task)
        {
            int firstIndex = (int)FirstIndex.GetValue().NumberValue;
            int lastIndex = (int)LastIndex.GetValue().NumberValue;
            int step = (int)Step.GetValue().NumberValue;

            if (task.ShouldBreak)
            {
                _firstRun = true;
                task.ShouldBreak = false;
                return Completed;
            }

            if (_firstRun)
            {
                Debug.Log($"first run: step {step}");
                task.EnterLoop(Enter);
                _index = firstIndex;

                _firstRun = false;
                return LoopBody;
            }

            _index += step;

            if (_index > lastIndex)
            {
                _firstRun = true;
                return Completed;
            }
            
            task.EnterLoop(Enter);
            return LoopBody;
        }
    }
}