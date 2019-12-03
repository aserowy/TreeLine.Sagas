using System;

namespace TreeLine.Sagas
{
    public interface ISagaCommand
    {
        Guid ReferenceId { get; }
        Guid TransactionId { get; }
    }
}