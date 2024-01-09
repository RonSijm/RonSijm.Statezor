namespace RonSijm.Statezor.Models;

public class Effect<T> : IEffect
{
    public virtual Func<Type, bool> TypeCriteria { get; set; }
    public virtual Func<T, bool> Criteria { get; set; }
    public virtual Func<T, Task> Action { get; set; }
}

public abstract class Reducer<TOut, T> : Effect<T>
{
    public override Func<Type, bool> TypeCriteria => type => type == typeof(TOut);

    public IState<TOut> CurrentState { get; }

    protected Reducer(IState<TOut> currentState)
    {
        CurrentState = currentState;
    }

    public override Func<T, Task> Action => input =>
    {
        CurrentState.Publish(GetValue(input));
        return Task.CompletedTask;
    };

    protected abstract TOut GetValue(T input);
}

public abstract class FuncReducer<TOut, T> : Effect<T>
{
    private readonly IState<TOut> _currentState;

    protected FuncReducer(IState<TOut> currentState)
    {
        _currentState = currentState;
    }

    public override Func<T, Task> Action => input =>
    {
        _currentState.Publish(Factory(input));
        return Task.CompletedTask;
    };

    protected abstract Func<T, TOut> Factory { get; set; }
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