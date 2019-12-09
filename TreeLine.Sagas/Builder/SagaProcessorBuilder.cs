using System;
using System.Collections.Generic;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaProcessorBuilder
    {
        ISagaVersionBuilder AddVersion(string version);

        ISagaProcessor Build();
    }

    internal sealed class SagaProcessorBuilder : ISagaProcessorBuilder
    {
        private readonly ISagaVersionFactory _versionFactory;
        private readonly ISagaServiceProvider _serviceProvider;
        private readonly IList<SagaVersionBuilder> _versionBuilders;

        public SagaProcessorBuilder(
            ISagaVersionFactory versionFactory, 
            ISagaServiceProvider serviceProvider)
        {
            _versionFactory = versionFactory;
            _serviceProvider = serviceProvider;

            _versionBuilders = new List<SagaVersionBuilder>();
        }

        public ISagaVersionBuilder AddVersion(string version)
        {
            var sagaVersion = _versionFactory.Create(version);
            var versionBuilder = new SagaVersionBuilder(sagaVersion);

            _versionBuilders.Add(versionBuilder);

            return versionBuilder;
        }

        public ISagaProcessor Build()
        {
            if (_versionBuilders.Count.Equals(0))
            {
                throw new InvalidOperationException("No version for processor configured.");
            }

            var processor = _serviceProvider.ResolveProcessor();
            foreach (var versionBuilder in _versionBuilders)
            {
                processor.AddSteps(versionBuilder.Version, versionBuilder.Steps);
            }

            return processor;
        }
    }
}