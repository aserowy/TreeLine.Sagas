using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas.ReferenceStore
{
    public sealed class EmptyReferenceStore : IReferenceStore
    {
        public Task AddReferences(params ISagaReference[] eventReference)
        {
            return Task.CompletedTask;
        }

        public Task<IList<ISagaReference>?> GetReferencesAsync(Guid referenceId)
        {
            return Task.FromResult<IList<ISagaReference>?>(null);
        }
    }
}