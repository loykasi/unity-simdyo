using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "UIInputCollection", menuName = "Scriptable Objects/Input Collection")]
    public class UIInputData : ScriptableObject
    {
        [System.Serializable]
        public class UIInput
        {
            public ScriptDataType Type;
            public BaseInput InputPrefab;
        }

        public UIInput[] Inputs;

        public BaseInput VariableNameInputPrefab;
        public BaseInput GlobalVariableNameInputPrefab;
        public BaseInput ListInputPrefab;
        public BaseInput KeyInputPrefab;
        public BaseInput CollisionLayerInput;

        private UIInput GetUIInput(DataType type)
        {
            for (int i = 0; i < Inputs.Length; i++)
            {
                if (Inputs[i].Type.Type == type)
                {
                    return Inputs[i];
                }
            }

            return null;
        }

        public BaseInput GetInputInstance(DataType type)
        {
            BaseInput inputPrefab = GetPrefab(type);
            if (inputPrefab == null)
            {
                return null;
            }

            BaseInput inputObject = Instantiate(inputPrefab);

            if (type == DataType.Entity)
            {
                var options = ObjectManager.Instance.GetEntityOptions();
                var entityInput = (EntityInput)inputObject;
                entityInput.Init(options);
            }

            return inputObject;
        }

        public BaseInput GetPrefab(DataType type)
        {
            BaseInput inputPrefab = GetUIInput(type).InputPrefab;
            if (inputPrefab == null)
            {
                return null;
            }

            BaseInput inputObject = Instantiate(inputPrefab);

            if (type == DataType.Entity)
            {
                var options = ObjectManager.Instance.GetEntityOptions();
                var entityInput = (EntityInput)inputObject;
                entityInput.Init(options);
            }

            return inputObject;
        }

        public BaseInput GetPrefab(ScriptDataType type)
        {
            if (type.Kind == DataKind.List)
            {
                return ListInputPrefab;
            }
            return GetUIInput(type.Type).InputPrefab;
        }

        public BaseInput Get(ScriptDataType type, SceneEntity entity = null)
        {
            BaseInput inputPrefab = GetPrefab(type);
            if (inputPrefab == null)
            {
                return null;
            }

            BaseInput inputObject = Instantiate(inputPrefab);

            if (type.Kind != DataKind.List && type.Type == DataType.Entity)
            {
                var options = ObjectManager.Instance.GetEntityOptions(entity);
                var entityInput = (EntityInput)inputObject;
                entityInput.Init(options);
            }

            if (type.Kind == DataKind.List)
            {
                ListInput input = (ListInput)inputObject;
                input.ListType = type.Type;
            }

            return inputObject;
        }

        public BaseInput Get(InputValueTypes inputType, SceneEntity entity)
        {
            if (inputType == InputValueTypes.Variable)
            {
                BaseInput input = Instantiate(VariableNameInputPrefab);
                var variableInput = (VariableNameInput)input;
                variableInput.Init(entity.Script);
                return input;
            }

            if (inputType == InputValueTypes.GlobalVariable)
            {
                BaseInput input = Instantiate(GlobalVariableNameInputPrefab);
                return input;
            }

            if (inputType == InputValueTypes.Key)
            {
                BaseInput input = Instantiate(KeyInputPrefab);
                return input;
            }

            if (inputType == InputValueTypes.CollisionLayer)
            {
                BaseInput input = Instantiate(CollisionLayerInput);
                return input;
            }

            if (inputType == InputValueTypes.SignalEntity)
            {
                BaseInput inputPrefab = GetPrefab(DataType.Entity);
                if (inputPrefab == null)
                {
                    return null;
                }

                BaseInput inputObject = Instantiate(inputPrefab);
                var options = ObjectManager.Instance.GetSignalEntityOptions();
                var entityInput = (EntityInput)inputObject;
                entityInput.Init(options);
                return entityInput;
            }

            DataType type = inputType switch
            {
                InputValueTypes.String => DataType.String,
                InputValueTypes.Number => DataType.Number,
                InputValueTypes.Boolean => DataType.Boolean,
                InputValueTypes.Entity => DataType.Entity,
                InputValueTypes.Color => DataType.Color,
                _ => throw new System.NotImplementedException(),
            };

            return Get(new ScriptDataType(type, DataKind.Simple), entity);
        }
    }
}