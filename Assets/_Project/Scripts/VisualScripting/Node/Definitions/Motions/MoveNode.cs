using UnityEngine;

namespace Loykas.Scripting
{
    class MoveNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Entity;
        public InputValue X;
        public InputValue Y;
        public InputValue Duration;

        private bool _isFirstFrame = true;
        private float _time;
        private float _duration;
        private Vector3 _start;
        private Vector3 _target;

        public override ScriptNode Create()
        {
            return new MoveNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Move);
            Exit = CreateOutputTrigger(nameof(Exit));

            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    HideLabel = true
                }
            )
            .UseInput()
            .NullMeanSelf();

            X = CreateInputValue
            (
                nameof(X),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );

            Y = CreateInputValue
            (
                nameof(Y),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );

            Duration = CreateInputValue
            (
                nameof(Duration),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            ).UseInput();
        }

        public OutputTrigger Move(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            if (entity == null)
            {
                return Exit;
            }

            if (_isFirstFrame)
            {
                _duration = Duration.GetValue().NumberValue;
                _time = _duration;
                _isFirstFrame = false;

                _start = entity.Position;
                _target.x = X.GetValue().NumberValue;
                _target.y = Y.GetValue().NumberValue;
            }

            if (_time > 0)
            {
                _time -= Time.deltaTime;
                
                entity.Position = Vector3.Lerp(_target, _start , _time / _duration);

                return null;
            }
            _isFirstFrame = true;
            entity.Position = _target;

            return Exit;
        }
    }
}