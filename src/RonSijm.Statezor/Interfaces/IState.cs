namespace RonSijm.Statezor.Interfaces;

public interface IState<T> : IState
{
    T Value { get; set; }

    Type IState.ForType => typeof(T);
    List<Effect<T>> Effects { get; set; }

    void Update(Action<T> update);
    void Publish(T state);
}

public interface IState
{
    void Subscribe(ComponentBase subscriberComponent);

    Type ForType { get; }
    void Unsubscribe(ComponentBase subscriberComponent);
};