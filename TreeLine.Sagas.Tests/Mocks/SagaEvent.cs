using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaEvent : ISagaEvent
    {
        public SagaEvent()
        {
            ReferenceId = Guid.NewGuid();
            TransactionId = Guid.NewGuid();
        }

        public Guid ReferenceId { get; set; }

        public Guid TransactionId { get; set; }
    }
}