namespace RonSijm.Statezor.Models;

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