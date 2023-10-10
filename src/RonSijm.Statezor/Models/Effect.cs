namespace RonSijm.Statezor.Models;

public interface IEffect
{
    Func<Type, bool> Criteria { get; set; }
}

public class Effect<T> : IEffect
{
    public Func<Type, bool> Criteria { get; set; }
    public Func<T, Task> Action { get; set; }
}

public interface IEffectResult : IEffect
{
    object GetValue();
}

// ReSharper disable once UnusedTypeParameter
public class Effect<T, TOut> : IEffectResult
{
    public Func<Type, bool> Criteria { get; set; }
    public Func<TOut> Action { get; set; }

    public object GetValue()
    {
        return Action.Invoke();
    }
}