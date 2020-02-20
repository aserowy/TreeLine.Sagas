using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.ReferenceStore;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddReferenceStore<TReferenceStore>(this IConfiguration configuration) where TReferenceStore : class, IReferenceStore
        {
            if (configuration is Configuration casted)
            {
                casted.Add(services => services.AddTransient<IReferenceStore, TReferenceStore>());
            }

            return configuration;
        }
    }
}
