using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    public sealed class SagaEvent01 : ISagaEvent
    {
        public SagaEvent01()
        {
            ReferenceId = Guid.NewGuid();
            TransactionId = Guid.NewGuid();
        }

        public Guid ReferenceId { get; set; }
        public Guid TransactionId { get; set; }
    }

    internal sealed class SagaEvent02 : ISagaEvent
    {
        public Guid ReferenceId => Guid.Empty;
        public Guid TransactionId => Guid.Empty;
    }
}