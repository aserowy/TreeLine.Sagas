using System;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.ReferenceStore
{
    public interface ISagaReference
    {
        ISagaVersion Version { get; }
        int StepIndex { get; }
        SagaMessageType MessageType { get; }
        Guid ReferenceId { get; }
        Guid TransactionId { get; }
    }

    public sealed class SagaReference : ISagaReference
    {
        public SagaReference(ISagaVersion sagaVersion, int stepIndex, SagaMessageType messageType, Guid referenceId, Guid transactionId)
        {
            Version = sagaVersion;
            StepIndex = stepIndex;
            MessageType = messageType;
            ReferenceId = referenceId;
            TransactionId = transactionId;
        }

        public ISagaVersion Version { get; }
        public int StepIndex { get; }
        public SagaMessageType MessageType { get; }
        public Guid ReferenceId { get; }
        public Guid TransactionId { get; }
    }
}