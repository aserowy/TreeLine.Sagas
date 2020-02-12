using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Processing
{
    public interface ISagaProcessor
    {
        void AddSteps(ISagaVersion version, IList<ISagaStepConfiguration> configurations);

        Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent);
    }

    internal sealed class SagaProcessor : ISagaProcessor
    {
        private readonly ISagaVersionStepResolver _resolver;
        private readonly ISagaServiceProvider _provider;
        private readonly ISagaProcess _process;
        private readonly ILoggerAdapter<SagaProcessor> _logger;
        private readonly IDictionary<ISagaVersion, IList<ISagaStepConfiguration>> _steps;

        public SagaProcessor(
            ISagaVersionStepResolver resolver,
            ISagaServiceProvider provider,
            ISagaProcess process,
            ILoggerAdapter<SagaProcessor> logger)
        {
            _resolver = resolver;
            _provider = provider;
            _process = process;
            _logger = logger;

            _steps = new Dictionary<ISagaVersion, IList<ISagaStepConfiguration>>();
        }

        public void AddSteps(ISagaVersion version, IList<ISagaStepConfiguration> configurations)
        {
            if (version is null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (configurations is null)
            {
                throw new ArgumentNullException(nameof(configurations));
            }

            if (_steps.ContainsKey(version))
            {
                _steps[version] = configurations;

                _logger.LogWarning($"Version {version} was already added. The given configuration is now replaced.");
            }
            else
            {
                _steps.Add(version, configurations);
            }
        }

        public async Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent)
        {
            if (sagaEvent is null)
            {
                throw new ArgumentNullException(nameof(sagaEvent));
            }

            if (_steps.Count.Equals(0))
            {
                throw new InvalidOperationException($"No steps configured for reference id {sagaEvent.ProcessId}.");
            }

            var configuration = await _resolver
                .ResolveAsync(sagaEvent, _steps)
                .ConfigureAwait(false);

            var step = configuration.Create(_provider);

            return await _process
                .RunAsync(sagaEvent, step)
                .ConfigureAwait(false);
        }
    }
}