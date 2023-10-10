using System.Globalization;

namespace RonSijm.Statezor.Services;

public static class StateChangedAccessor
{
    private static readonly Lazy<MethodInfo> AccessorFactory = new(Accessor);

    private static MethodInfo Accessor()
    {
        var componentBase = typeof(ComponentBase);
        var methodInfo = componentBase.GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic);

        return methodInfo;
    }

    public static void OnStateHasChanged(this ComponentBase componentBase)
    {
        AccessorFactory.Value.Invoke(componentBase, BindingFlags.Instance | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
    }
}