using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Validation.Tests.Mocks
{
    public sealed class SagaEvent01 : ISagaEvent
    {
        public SagaEvent01()
        {
            ProcessId = Guid.NewGuid();
            TransactionId = Guid.NewGuid();
        }

        public Guid ProcessId { get; set; }
        public Guid TransactionId { get; set; }
    }

    internal sealed class SagaEvent02 : ISagaEvent
    {
        public Guid ProcessId => Guid.Empty;
        public Guid TransactionId => Guid.Empty;
    }
}