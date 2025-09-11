using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class OperatorHandler
{
    private readonly Dictionary<OperatorKey, Func<object, object, object>> _operators = new();

    public object Operate(object a, object b)
    {
        OperatorKey key = new(a.GetType(), b.GetType());
        Debug.Log($"{a.GetType()} + {b.GetType()}");
        return _operators[key](a, b);
    }

    public void Operator<TLeft, TRight>(Func<TLeft, TRight, object> logic)
    {
        OperatorKey key = new(typeof(TLeft), typeof(TRight));
        if (_operators.ContainsKey(key))
        {
            throw new ArgumentException($"Operator ({typeof(TLeft)}, {typeof(TRight)}) is already registered");
        }

        _operators.Add(key, (left, right) => logic((TLeft)left, (TRight)right));
    }

    private struct OperatorKey : IEquatable<OperatorKey>
    {
        public Type Left;
        public Type Right;

        public OperatorKey(Type left, Type right)
        {
            Left = left;
            Right = right;
        }

        public bool Equals(OperatorKey other)
        {
            return Left == other.Left && Right == other.Right;
        }

        public override bool Equals(object obj)
        {
            if (obj is OperatorKey other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Left?.GetHashCode() ?? 0);
                hash = hash * 23 + (Right?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}

