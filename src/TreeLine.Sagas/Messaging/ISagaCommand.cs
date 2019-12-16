using System;

namespace TreeLine.Sagas.Messaging
{
    public interface ISagaCommand
    {
        Guid ReferenceId { get; }
        Guid TransactionId { get; }
    }
}