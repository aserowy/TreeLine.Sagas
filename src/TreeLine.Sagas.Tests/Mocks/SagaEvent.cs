using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    public sealed class SagaEvent01 : ISagaEvent
    {
        public Guid ProcessId => Guid.Empty;
        public Guid TransactionId => Guid.Empty;
    }

    internal sealed class SagaEvent02 : ISagaEvent
    {
        public Guid ProcessId => Guid.Empty;
        public Guid TransactionId => Guid.Empty;
    }
}