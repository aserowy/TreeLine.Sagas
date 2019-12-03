using System;

namespace TreeLine.Sagas.Messaging
{
    public interface ISagaEvent
    {
        Guid ReferenceId { get; }
        Guid TransactionId { get; }
    }
}