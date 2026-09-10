using System;

public abstract class EntityProperty
{
    public DelegateList ChangedEvent = new();

    public abstract bool IsEqual(EntityProperty other);
    public abstract object Get();
    public abstract void Set(object value);

    public virtual void OnChanged()
    {
        ChangedEvent?.Invoke();
    }
}

public class EntityProperty<T> : EntityProperty
{
    private Func<T> _getter;
    private Action<T> _setter;

    public EntityProperty(Func<T> getter, Action<T> setter)
    {
        _getter = getter;
        _setter = setter;
    }

    public override object Get()
    {
        return _getter != null ? _getter.Invoke() : default;
    }

    public override bool IsEqual(EntityProperty other)
    {
        if (other is EntityProperty<T> property)
        {
            return Get().Equals(property.Get());
        }

        return false;
    }

    public override void Set(object value)
    {
        _setter?.Invoke((T)value);
    }
}