namespace Loykas.Scripting
{
    public class FunctionCallNode : ScriptNode
    {
        public override bool ShouldIncludeInMenu => false;
        public override bool ShouldLocalized => false;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public ScriptFunction Function;

        private bool _firstRun = true;
        private NodeTask _task = new();

        public override ScriptNode Create()
        {
            return new FunctionCallNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), TriggerFunction);
            Exit = CreateOutputTrigger(nameof(Exit), PortSettings.Default);
        }

        public override string GetNameKey()
        {
            return Function != null ? Function.Name : string.Empty;
        }

        public override void Reset()
        {
            _task.Stop();
            _firstRun = true;
        }

        public void Init(ScriptFunction function)
        {
            Function = function;

            Function.OnUpdated += OnFunctionUpdated;

            OnFunctionUpdated();
        }

        public OutputTrigger TriggerFunction(NodeTask task)
        {
            if (_firstRun)
            {
                _firstRun = false;

                _task.From = Function.StartNode.Exit;
                _task.Trigger = Function.StartNode.Exit.Invoke();
            }

            for (int i = 0; i < Function.Inputs.Count; i++)
            {
                FunctionInput argument = Function.Inputs[i];
                InputValue input = ValueInputs[i];
                argument.Value = input.GetValue();
            }

            _task.Invoke(Flow);
            if (!_task.IsDone)
            {
                return null;
            }

            _firstRun = true;
            return Exit;
        }

        private void OnFunctionUpdated()
        {
            for (int i = 0; i < ValueInputs.Count; i++)
            {
                Flow.Disconnect(ValueInputs[i]);
            }
            ValueInputs.Clear();

            for (int i = 0; i < Function.Inputs.Count; i++)
            {
                FunctionInput argument = Function.Inputs[i];
                CreateInputValue
                (
                    argument.Name,
                    argument.Type,
                    new PortSettings
                    {
                        IsLocalizationDisabled = true
                    }
                );
            }

            OnNodeUpdated?.Invoke();
        }
    }
}