using System;

namespace TreeLine.Sagas.Messaging
{
    public interface ISagaEvent
    {
        Guid ProcessId { get; }
        Guid TransactionId { get; }
    }
}