using Microsoft.Extensions.DependencyInjection;
using System;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;

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

        public ISagaStep<TEvent> Resolve<TEvent, TSagaStep>()
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep<TEvent>
        {
            return _serviceProvider.GetRequiredService<TSagaStep>();
        }
    }
}