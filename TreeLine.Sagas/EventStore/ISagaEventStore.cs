using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.EventStore
{
    public interface ISagaEventStore
    {
        Task<IList<ISagaReference>?> GetEventsAsync(Guid referenceId);
    }
}