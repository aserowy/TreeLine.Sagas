using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.EventStore.Accessing
{
    public interface IEventStoreRepository
    {
        Task<bool> CreateOrUpdateAsync(IEnumerable<ISagaReference> references, TimeSpan? timeToLive = null);
        Task<IEnumerable<ISagaReference>> GetAsync(Guid referenceId);
    }
}
