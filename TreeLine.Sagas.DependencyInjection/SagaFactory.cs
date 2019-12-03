using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TreeLine.Sagas.DependencyInjection
{
    internal sealed class SagaFactory : ISagaFactory
    {
        private readonly IServiceCollection _services;

        public SagaFactory(IServiceCollection services)
        {
            _services = services;
        }

        public ISaga<TProfile> Create<TProfile>() where TProfile : ISagaProfile
        {
            return _services
                .BuildServiceProvider()
                .GetRequiredService<ISaga<TProfile>>();
        }
    }
}