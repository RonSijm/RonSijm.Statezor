using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace RonSijm.Statezor.Helpers;

public static class AddLocalStorageState
{
    public static IServiceProvider UseLocalStorage<T>(this IServiceProvider serviceProvider, Action<T> additionalEffect = null, string name = null)
    {
        name ??= typeof(T).FullName;

        CreateGetter(serviceProvider, additionalEffect, name);
        CreateSetter<T>(serviceProvider, name);

        return serviceProvider;
    }

    private static void CreateGetter<T>(IServiceProvider serviceProvider, Action<T> additionalEffect, string name)
    {
        var defaultStateContainer = serviceProvider.GetService<DefaultStateContainer>();
        defaultStateContainer.Effects.Add(new Effect<T, T>
        {
            Action = () =>
            {
                var syncLocalStorageService = serviceProvider.GetService<ISyncLocalStorageService>();
                var result = syncLocalStorageService.GetItem<T>(name);

                additionalEffect?.Invoke(result);

                return result;
            },
            Criteria = type => type == typeof(T)
        });
    }

    private static void CreateSetter<T>(IServiceProvider serviceProvider, string name)
    {
        var stateStore = serviceProvider.GetService<StateStore>();

        stateStore.Effects.Add(new Effect<T>
        {
            Action = async obj =>
            {
                var localStorageService = serviceProvider.GetService<ILocalStorageService>();
                await localStorageService.SetItemAsync(name, obj);
            },
            Criteria = type => type == typeof(T)
        });
    }
}