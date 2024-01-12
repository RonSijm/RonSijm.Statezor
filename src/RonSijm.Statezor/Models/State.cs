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
        PublishState = StateType.Publishing;
        Console.WriteLine($"Publishing {typeof(T)}");

        Value = state;
        _stateStore.SetState(Value);
        PublishState = StateType.Published;
    }

    public StateType PublishState { get; set; }

    public async Task Publish(Func<Task<T>> stateFactory)
    {
        PublishState = StateType.Publishing;
        var state = await stateFactory();
        Publish(state);
    }

    public T Value { get; set; }
    public List<Effect<T>> Effects { get; set; } = new();
}