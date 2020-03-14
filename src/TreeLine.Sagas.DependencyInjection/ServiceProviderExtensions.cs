using Microsoft.Extensions.DependencyInjection;
using System;
using TreeLine.Sagas.Validating;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider ValidateSagas(this IServiceProvider provider)
        {
            provider
                .GetRequiredService<IValidator>()
                .Validate();

            return provider;
        }
    }
}
