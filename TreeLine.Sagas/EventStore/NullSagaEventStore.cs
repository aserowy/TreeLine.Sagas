using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.EventStore
{
    internal sealed class NullSagaEventStore : ISagaEventStore
    {
        public Task<IList<ISagaReference>?> GetEventsAsync(Guid referenceId)
        {
            return Task.FromResult<IList<ISagaReference>?>(null);
        }
    }
}