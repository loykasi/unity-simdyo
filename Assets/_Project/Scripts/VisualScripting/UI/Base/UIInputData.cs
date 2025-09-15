using UnityEngine;

[CreateAssetMenu(fileName = "UIInputCollection", menuName = "Scriptable Objects/Input Collection")]
public class UIInputData : ScriptableObject
{
    [System.Serializable]
    public class UIInput
    {
        public DataType Type;
        public BaseInput InputPrefab;
    }

    public UIInput[] Inputs;

    public BaseInput VariableNameInputPrefab;

    private UIInput GetUIInput(DataType type)
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            if (Inputs[i].Type == type)
            {
                return Inputs[i];
            }
        }

        return null;
    }

    public BaseInput GetPrefab(DataType type)
    {
        return GetUIInput(type).InputPrefab;
    }

    public BaseInput Get(DataType type, SceneEntity entity)
    {
        BaseInput input = GetPrefab(type);
        if (input == null)
        {
            return null;
        }

        BaseInput inputObject = Instantiate(input);

        if (type == DataType.Entity)
        {
            var options = ObjectManager.Instance.GetEntityOptions();
            var entityInput = (EntityInput)inputObject;
            entityInput.Init(options);
        }

        return inputObject;
    }

    public BaseInput Get(DataType type, DataType? subType, SceneEntity entity)
    {
        if (type != DataType.List)
        {
            return Get(type, entity);
        }

        if (subType == null)
        {
            return null;
        }

        BaseInput inputPrefab = GetPrefab(type);

        if (inputPrefab == null)
        {
            return null;
        }

        ListInput input = (ListInput)Instantiate(inputPrefab);
        input.InputPrefab = GetPrefab(subType.Value);

        return input;
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

        return Get(type, null, null);
    }
}