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

    public BaseInput Get(DataType type)
    {
        BaseInput input = GetPrefab(type);
        if (input == null)
        {
            return null;
        }

        return Instantiate(input);
    }

    public BaseInput Get(DataType type, DataType? subType)
    {
        if (type != DataType.List)
        {
            return Get(type);
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
}