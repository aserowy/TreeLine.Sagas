using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.EventStore
{
    public interface ISagaEventStore
    {
        Task AddReferences(params ISagaReference[] eventReference);
        Task<IList<ISagaReference>?> GetReferencesAsync(Guid referenceId);
    }
}