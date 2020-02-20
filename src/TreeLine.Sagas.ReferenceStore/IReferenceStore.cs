using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.ReferenceStore
{
    public interface IReferenceStore
    {
        Task AddReferences(params ISagaReference[] eventReference);
        Task<IList<ISagaReference>?> GetReferencesAsync(Guid referenceId);
    }
}