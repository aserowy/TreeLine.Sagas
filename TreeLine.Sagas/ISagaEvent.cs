using System;

namespace TreeLine.Sagas
{
    public interface ISagaEvent
    {
        Guid ReferenceId { get; }
        Guid TransactionId { get; }
    }
}