namespace RonSijm.Statezor.Extensions;

public static class StatePublishExtension
{
    public static async Task IfUninitialized<T>(this IState<T> state, Func<Task<T>> stateFactory)
    {
        if (state.PublishState == StateType.Uninitialized)
        {
            await state.Publish(stateFactory);
        }
    }

    public static void PublishIfDifferent<T>(this IState<T> state, object stateValueObject)
    {
        if (stateValueObject is T stateValue)
        {
            PublishIfDifferent(state, stateValue);
        }
    }

    public static void PublishIfDifferent<T>(this IState<T> state, T stateValue)
    {
        if (state.Value == null && stateValue == null)
        {
            return;
        }

        if (state.Value == null || stateValue == null)
        {
            state.Publish(stateValue);
            return;
        }

        if (!state.Value.Equals(stateValue))
        {
            state.Publish(stateValue);
        }
    }
}