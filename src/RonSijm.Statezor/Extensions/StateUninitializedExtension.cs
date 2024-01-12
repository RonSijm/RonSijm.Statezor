namespace RonSijm.Statezor.Extensions;

public static class StateUninitializedExtension
{
    public static bool IsUninitialized(this IState state)
    {
        return state.PublishState == StateType.Uninitialized;
    }
}