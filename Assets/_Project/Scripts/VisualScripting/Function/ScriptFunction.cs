using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class ScriptFunction
    {
        public event Action OnUpdated;

        public string Name;

        public List<FunctionInput> Inputs = new();

        public bool HasReturnValue;
        public DataType ReturnType = DataType.String;

        
        // Node references
        public FunctionEnterNode StartNode;
        public ScriptNode ReturnNode;

        public void AddInput(string name)
        {
            FunctionInput input = new();
            Inputs.Add(input);

            input.Name = name;

            OnUpdated?.Invoke();
            
            StartNode.Init(this);
        }

        public void EditInput(int index, string name, ScriptDataType type)
        {
            FunctionInput input = Inputs[index];
            input.Name = name;
            input.Type = type;

            OnUpdated?.Invoke();
            Debug.Log($"Updated {index}");

            StartNode.Init(this);
        }

        public void DeleteInput(int index)
        {
            Inputs.RemoveAt(index);
            OnUpdated?.Invoke();

            StartNode.Init(this);
        }

        public void SetReturnValue(bool value)
        {
            HasReturnValue = value;
        }

        public void EditReturnValue(DataType type)
        {
            ReturnType = type;
        }

        public void EditName(string name)
        {
            Name = name;
            OnUpdated?.Invoke();

            StartNode.Init(this);
        }
    }
}