using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Processor
{
    public interface ISagaProcessor
    {
        void AddSteps(IList<ISagaStepConfiguration> configurations);

        Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent);
    }

    internal sealed class SagaProcessor : ISagaProcessor
    {
        private readonly ISagaServiceProvider _provider;
        private readonly ISagaProcess _process;

        private IList<ISagaStepConfiguration> _steps;

        public SagaProcessor(
            ISagaServiceProvider provider,
            ISagaProcess process)
        {
            _provider = provider;
            _process = process;

            _steps = new List<ISagaStepConfiguration>();
        }

        public void AddSteps(IList<ISagaStepConfiguration> configurations)
        {
            _steps = configurations ?? throw new ArgumentNullException(nameof(configurations));
        }

        public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent)
        {
            var step = ResolveStep(sagaEvent);

            return _process.RunAsync(sagaEvent, step);
        }

        private ISagaStepAdapter ResolveStep(ISagaEvent sagaEvent)
        {
            if (sagaEvent == null)
            {
                throw new ArgumentNullException(nameof(sagaEvent));
            }

            if (_steps.Count.Equals(0))
            {
                throw new InvalidOperationException($"No steps for processor of event {sagaEvent.GetType().Name} configured.");
            }

            foreach (var configuration in _steps)
            {
                if (configuration.IsResponsible(sagaEvent))
                {
                    return configuration.Create(_provider);
                }
            }

            throw new ArgumentOutOfRangeException($"No step for event type {sagaEvent.GetType().Name} found.");
        }
    }
}