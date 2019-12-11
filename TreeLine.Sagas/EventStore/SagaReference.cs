using System;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.EventStore
{
    public interface ISagaReference
    {
        Guid ReferenceId { get; }
        Guid TransactionId { get; }
        ISagaVersion Version { get; }
        int StepIndex { get; }
    }

    internal sealed class SagaReference : ISagaReference
    {
        public SagaReference(ISagaVersion sagaVersion, int stepIndex, Guid referenceId, Guid transactionId)
        {
            Version = sagaVersion;
            StepIndex = stepIndex;
            ReferenceId = referenceId;
            TransactionId = transactionId;
        }

        public Guid ReferenceId { get; }
        public Guid TransactionId { get; }
        public ISagaVersion Version { get; }
        public int StepIndex { get; }
    }
}