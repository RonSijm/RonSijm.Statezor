namespace RonSijm.Statezor.Models;

public class Effect<T> : IEffect
{
    public virtual Func<Type, bool> TypeCriteria { get; set; }
    public virtual Func<T, bool> Criteria { get; set; }
    public virtual Func<T, Task> Action { get; set; }
}

// ReSharper disable once UnusedTypeParameter
public class Effect<T, TOut> : IEffectResult
{
    public virtual Func<Type, bool> TypeCriteria { get; set; }
    public object GetValue()
    {
        return GetTypedValue();
    }

    public TOut GetTypedValue()
    {
        return Action.Invoke();
    }

    public virtual Func<T, bool> Criteria { get; set; }
    public virtual Func<TOut> Action { get; set; }


}