using System;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaReference : ISagaReference
    {
        public Guid ReferenceId { get; set; }
        public Guid TransactionId { get; set; }
        public ISagaVersion Version { get; set; }
        public int StepIndex { get; set; }
    }
}