using RonSijm.Statezor.Helpers;

namespace RonSijm.Statezor.Components;

/// <inheritdoc cref="PublicEventsComponent" />
/// <summary>
/// Class you can inherit from to automatically subscribe and unsubscribe states
/// </summary>
public abstract class AutoSubscribeComponent : PublicEventsComponent, IDisposable
{
    private List<IState> ComponentStates { get; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        // Find all state properties
        ComponentStates.AddRange(StateSubscriber.SubscribeWithOutput(this));
    }

    public void Dispose()
    {
        foreach (var state in ComponentStates)
        {
            state?.Unsubscribe(this);
        }

        GC.SuppressFinalize(this);
    }
}