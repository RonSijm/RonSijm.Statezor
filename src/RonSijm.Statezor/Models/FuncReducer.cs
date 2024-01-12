namespace RonSijm.Statezor.Models;

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