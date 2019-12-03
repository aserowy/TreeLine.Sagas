using Microsoft.Extensions.DependencyInjection;
using System;

namespace TreeLine.Sagas.DependencyInjection
{
    internal sealed class SagaServiceProvider : ISagaServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public SagaServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISagaProcessor ResolveProcessor()
        {
            return _serviceProvider.GetRequiredService<ISagaProcessor>();
        }

        public ISagaStep Resolve<TSagaStep>() where TSagaStep : ISagaStep
        {
            return _serviceProvider.GetRequiredService<TSagaStep>();
        }
    }
}