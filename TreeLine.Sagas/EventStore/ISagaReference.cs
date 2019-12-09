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
}