using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.ReferenceStore;

namespace TreeLine.Sagas.DependencyInjection.Tests.Mocks
{
    internal sealed class SagaReferenceStoreMock : IReferenceStore
    {
        public Task AddReferences(params ISagaReference[] eventReference)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ISagaReference>> GetReferencesAsync(Guid referenceId)
        {
            throw new NotImplementedException();
        }
    }
}
