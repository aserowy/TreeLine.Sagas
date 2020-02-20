using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.ReferenceStore.Accessing
{
    public interface IReferenceRepository
    {
        Task<bool> CreateOrUpdateAsync(IEnumerable<ISagaReference> references, TimeSpan? timeToLive = null);
        Task<IEnumerable<ISagaReference>> GetAsync(Guid referenceId);
    }
}
