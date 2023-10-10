namespace RonSijm.Statezor.Helpers;

public static class StateSubscriber
{
    const BindingFlags Flags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;

    public static IEnumerable<IState> SubscribeWithOutput(ComponentBase target)
    {
        var stateProperties = target.GetType().GetProperties(Flags).Where(t => typeof(IState).IsAssignableFrom(t.PropertyType)).ToList();

        foreach (var propertyInfo in stateProperties)
        {
            var state = (IState)propertyInfo.GetValue(target);
            yield return state;

            state?.Subscribe(target);
        }
    }

    public static void Subscribe(ComponentBase target)
    {
        var stateProperties = target.GetType().GetProperties(Flags).Where(t => typeof(IState).IsAssignableFrom(t.PropertyType)).ToList();

        foreach (var state in stateProperties.Select(propertyInfo => (IState)propertyInfo.GetValue(target)))
        {
            state?.Subscribe(target);
        }
    }
}