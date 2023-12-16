using Microsoft.Extensions.DependencyInjection;

namespace RonSijm.Statezor.Helpers
{
    public static class AddReducerExtension
    {
        public static IServiceProvider AddReducer<T, TReducer>(this IServiceProvider serviceProvider) where TReducer : Effect<T>
        {
            var state = serviceProvider.GetService<IState<T>>();
            var reducer = serviceProvider.GetService<TReducer>();

            state.Effects.Add(reducer);

            return serviceProvider;
        }
    }
}