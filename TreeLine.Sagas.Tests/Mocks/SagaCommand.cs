using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaCommand : ISagaCommand
    {
        public SagaCommand()
        {
            ReferenceId = Guid.NewGuid();
            TransactionId = Guid.NewGuid();
        }

        public Guid ReferenceId { get; set; }

        public Guid TransactionId { get; set; }
    }
}