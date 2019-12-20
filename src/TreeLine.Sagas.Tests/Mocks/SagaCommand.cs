using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaCommand : ISagaCommand
    {
        public SagaCommand()
        {
            ProcessId = Guid.NewGuid();
            TransactionId = Guid.NewGuid();
        }

        public Guid ProcessId { get; set; }

        public Guid TransactionId { get; set; }
    }
}