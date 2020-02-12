using System;
using System.Collections.Generic;
using TreeLine.Sagas.Processing;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Building
{
    public interface ISagaProcessorBuilder
    {
        ISagaVersionBuilder AddVersion(string version);

        ISagaProcessor Build();
    }

    internal sealed class SagaProcessorBuilder : ISagaProcessorBuilder
    {
        private readonly ISagaVersionFactory _versionFactory;
        private readonly ISagaProcessor _processor;
        private readonly IList<SagaVersionBuilder> _versionBuilders;

        public SagaProcessorBuilder(
            ISagaVersionFactory versionFactory,
            ISagaProcessor processor)
        {
            _versionFactory = versionFactory;
            _processor = processor;

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

            foreach (var versionBuilder in _versionBuilders)
            {
                _processor.AddSteps(versionBuilder.Version, versionBuilder.Steps);
            }

            return _processor;
        }
    }
}