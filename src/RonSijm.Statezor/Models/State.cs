namespace RonSijm.Statezor.Models;

public class State<T> : IState<T>
{
    private readonly StateStore _stateStore;

    public State(StateStore stateStore)
    {
        _stateStore = stateStore;
    }
    public void Subscribe(ComponentBase subscriberComponent)
    {
        _stateStore.AddSubscription<T>(subscriberComponent);
    }

    public void Unsubscribe(ComponentBase subscriberComponent)
    {
        _stateStore.RemoveSubscription(subscriberComponent);
    }

    public void Update(Action<T> update)
    {
        Value ??= Activator.CreateInstance<T>();

        if (update == null)
        {
            _stateStore.SetState<T>(default);
        }
        else
        {
            update(Value);
            _stateStore.SetState(Value);
        }
    }

    public void Publish(T state)
    {
        Console.WriteLine($"Publishing {typeof(T)}");

        Value = state;
        _stateStore.SetState(Value);
    }

    public T Value { get; set; }
    public List<Effect<T>> Effects { get; set; } = new();
}