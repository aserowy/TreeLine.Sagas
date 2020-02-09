using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.EventStore;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddEventStore<TEventStore>(this IConfiguration configuration) where TEventStore : class, ISagaEventStore
        {
            if (configuration is Configuration casted)
            {
                casted.Add(services => services.AddTransient<ISagaEventStore, TEventStore>());
            }

            return configuration;
        }
    }
}
