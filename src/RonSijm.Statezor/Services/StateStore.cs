namespace RonSijm.Statezor.Services;

public class StateStore
{
    public StateStore(DefaultStateContainer defaultState)
    {
        _defaultState = defaultState;
    }

    private readonly List<(WeakReference<ComponentBase> Subscriber, Type TargetType)> _subscribers = new();
    private readonly List<IState> _states = new();

    public readonly List<IEffect> Effects = new();

    private readonly DefaultStateContainer _defaultState;

    public IState<T> GetState<T>() where T : new()
    {
        if (_states.FirstOrDefault(x => x.ForType == typeof(T)) is IState<T> existingState)
        {
            return existingState;
        }

        var state = _defaultState.GetState<T>(this);
        _states.Add(state);

        return state;
    }

    public IState GetState(Type type)
    {
        if (_states.FirstOrDefault(x => x.ForType == type) is { } existingState)
        {
            return existingState;
        }

        var state = _defaultState.GetState(type, this);
        _states.Add(state);

        return state;
    }

    public void SetState<T>(T state)
    {
        var stateObject = ActuallySetState(state);
        NotifySubscribers<T>();

        if (stateObject.Effects == null)
        {
            var effects = Effects.Where(x => x.Criteria.Invoke(typeof(T)));
            stateObject.Effects = effects.Cast<Effect<T>>().ToList();
        }

        foreach (var effect in stateObject.Effects)
        {
            effect.Action.Invoke(stateObject.Value);
        }
    }

    private void NotifySubscribers<T>()
    {
        var relevantSubscribers = _subscribers.Where(x => x.TargetType == typeof(T)).ToList();

        foreach (var (weakSubscriber, targetType) in relevantSubscribers)
        {
            if (weakSubscriber.TryGetTarget(out var subscriber))
            {
                if (subscriber is PublicEventsComponent actualSubscriber)
                {
                    actualSubscriber.OnStateHasChanged();
                }
                else
                {
                    subscriber.OnStateHasChanged();
                }
            }
        }
    }

    private IState<T> ActuallySetState<T>(T state)
    {
        if (_states.FirstOrDefault(x => x.ForType == typeof(T)) is IState<T> existing)
        {
            existing.Value = state;
            return existing;
        }

        var newState = new State<T>(this)
        {
            Value = state
        };

        _states.Add(newState);

        return newState;
    }

    public void AddSubscription<T>(ComponentBase subscriberComponent)
    {
        if (!_subscribers.Any(x =>
            {
                if (x.TargetType != typeof(T))
                {
                    return false;
                }

                var canGetTarget = x.Subscriber.TryGetTarget(out var target);

                if (!canGetTarget)
                {
                    // Debug
                }

                if (subscriberComponent == target)
                {
                    return true;
                }

                return false;
            }))
        {
            _subscribers.Add((new WeakReference<ComponentBase>(subscriberComponent), typeof(T)));
        }
    }

    public void RemoveSubscription(ComponentBase subscriberComponent)
    {
        _subscribers.RemoveAll(x => x.Subscriber.TryGetTarget(out var target) && target == subscriberComponent);
    }
}