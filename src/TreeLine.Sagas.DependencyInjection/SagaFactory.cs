using Microsoft.Extensions.DependencyInjection;
using System;

namespace TreeLine.Sagas.DependencyInjection
{
    internal sealed class SagaFactory : ISagaFactory
    {
        private readonly IServiceProvider _provider;

        public SagaFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ISaga<TProfile> Create<TProfile>() where TProfile : ISagaProfile
        {
            return _provider.GetRequiredService<ISaga<TProfile>>();
        }
    }
}