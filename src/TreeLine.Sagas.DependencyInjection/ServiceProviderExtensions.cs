using Microsoft.Extensions.DependencyInjection;
using System;
using TreeLine.Sagas.Validation;

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
