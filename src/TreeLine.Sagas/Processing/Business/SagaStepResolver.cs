using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.ReferenceStore;

namespace TreeLine.Sagas.Processing.Business
{
    internal interface ISagaStepResolver
    {
        Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, ISagaStepConfiguration> Create();
    }

    internal sealed class SagaStepResolver : ISagaStepResolver
    {
        public Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, ISagaStepConfiguration> Create()
        {
            return _resolve;
        }

        private static readonly Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, ISagaStepConfiguration> _resolve =
            (sagaEvent, references, configurations) =>
            {
                if (configurations.Count.Equals(0))
                {
                    throw new ArgumentNullException(nameof(configurations));
                }

                return _getConfigurationFunc(sagaEvent, _getValidConfigurationRangeFunc(sagaEvent, references, configurations));
            };

        private static readonly Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, IList<ISagaStepConfiguration>> _getValidConfigurationRangeFunc =
            (sagaEvent, references, configurations) =>
            {
                if (references is null)
                {
                    return configurations;
                }

                references = references
                    .Where(rfrnc => rfrnc.ReferenceId.Equals(sagaEvent.ProcessId))
                    .ToList();

                if (references.Count.Equals(0))
                {
                    return configurations;
                }

                var reference = references.LastOrDefault(rfrnc => rfrnc.TransactionId.Equals(sagaEvent.TransactionId));
                if (reference is null)
                {
                    throw new InvalidOperationException($"No reference for event with transaction id {sagaEvent.TransactionId} found.");
                }

                if (configurations.Count <= reference.StepIndex)
                {
                    throw new ArgumentOutOfRangeException($"Reference step index {reference.StepIndex} could not resolve a step for reference id {reference.TransactionId}");
                }

                return configurations
                    .Skip(reference.StepIndex + 1)
                    .ToList();
            };

        private static readonly Func<ISagaEvent, IList<ISagaStepConfiguration>, ISagaStepConfiguration> _getConfigurationFunc =
            (sagaEvent, configurations) =>
            {
                foreach (var configuration in configurations)
                {
                    if (configuration.IsResponsible(sagaEvent))
                    {
                        return configuration;
                    }
                }

                throw new InvalidOperationException($"No step for event type {sagaEvent.GetType().Name} found.");
            };
    }
}