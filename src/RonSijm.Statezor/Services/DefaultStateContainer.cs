namespace RonSijm.Statezor.Services;

public class DefaultStateContainer
{
    private static readonly MethodInfo Method = typeof(DefaultStateContainer).GetMethods().FirstOrDefault(x => x.Name == nameof(GetState) && x.IsGenericMethod);

    public readonly List<IEffectResult> Effects = new();

    public IState<T> GetState<T>(StateStore stateStore)
    {
        var state = new State<T>(stateStore);

        var effect = Effects.FirstOrDefault(x => x.Criteria.Invoke(typeof(T)));

        if (effect != null)
        {
            var stateValue = (T)effect.GetValue();
            state.Value = stateValue;
        }

        return state;
    }

    public IState GetState(Type stateType, StateStore stateStore)
    {
        var getMethod = Method.MakeGenericMethod(stateType);
        var result = getMethod.Invoke(this, new object[] { stateStore });

        var resultState = (IState)result;
        return resultState;
    }
}