using UnityEngine;

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
    public BaseInput ListInputPrefab;

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
        if (type.IsList)
        {
            return ListInputPrefab;
        }
        return GetUIInput(type.Type).InputPrefab;
    }

    public BaseInput Get(ScriptDataType type)
    {
        BaseInput inputPrefab = GetPrefab(type);
        if (inputPrefab == null)
        {
            return null;
        }

        BaseInput inputObject = Instantiate(inputPrefab);

        if (!type.IsList && type.Type == DataType.Entity)
        {
            var options = ObjectManager.Instance.GetEntityOptions();
            var entityInput = (EntityInput)inputObject;
            entityInput.Init(options);
        }

        if (type.IsList)
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

            var list = entity.Script.GetVariableOptions();
            variableInput.Init(list);

            return input;
        }

        DataType type = inputType switch
        {
            InputValueTypes.String => DataType.String,
            InputValueTypes.Number => DataType.Number,
            InputValueTypes.Boolean => DataType.Boolean,
            InputValueTypes.Entity => DataType.Entity,
            _ => throw new System.NotImplementedException(),
        };

        return Get(new ScriptDataType(type, false));
    }
}