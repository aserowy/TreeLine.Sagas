using System;

namespace TreeLine.Sagas.Messaging
{
    public interface ISagaCommand
    {
        Guid ProcessId { get; }
        Guid TransactionId { get; }
    }
}