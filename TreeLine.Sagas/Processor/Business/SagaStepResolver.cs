using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Processor.Business
{
    internal interface ISagaStepResolver
    {
        Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, ISagaStepConfiguration> Create();
    }

    internal sealed class SagaStepResolver : ISagaStepResolver
    {
        public Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, ISagaStepConfiguration> Create()
        {
            return Resolve;
        }

        private static readonly Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, ISagaStepConfiguration> Resolve =
            (sagaEvent, references, configurations) =>
            {
                if (configurations?.Count.Equals(0) != false)
                {
                    throw new ArgumentNullException(nameof(configurations));
                }

                return GetConfigurationFunc(sagaEvent, GetValidConfigurationRangeFunc(sagaEvent, references, configurations));
            };

        private static readonly Func<ISagaEvent, IList<ISagaReference>?, IList<ISagaStepConfiguration>, IList<ISagaStepConfiguration>> GetValidConfigurationRangeFunc =
            (sagaEvent, references, configurations) =>
            {
                if (references is null)
                {
                    return configurations;
                }

                var reference = references.SingleOrDefault(rfrnc => rfrnc.TransactionId.Equals(sagaEvent.TransactionId));
                if (configurations.Count >= reference.StepIndex)
                {
                    throw new ArgumentOutOfRangeException($"Reference step index {reference.StepIndex} could not resolve a step for reference id {reference.TransactionId}");
                }

                return configurations
                    .Skip(reference.StepIndex + 1)
                    .ToList();
            };

        private static readonly Func<ISagaEvent, IList<ISagaStepConfiguration>, ISagaStepConfiguration> GetConfigurationFunc =
            (sagaEvent, configurations) =>
            {
                foreach (var configuration in configurations)
                {
                    if (configuration.IsResponsible(sagaEvent))
                    {
                        return configuration;
                    }
                }

                throw new ArgumentOutOfRangeException($"No step for event type {sagaEvent.GetType().Name} found.");
            };
    }
}