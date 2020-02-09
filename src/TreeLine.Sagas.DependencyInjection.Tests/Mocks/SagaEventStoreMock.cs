using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.EventStore;

namespace TreeLine.Sagas.DependencyInjection.Tests.Mocks
{
    internal sealed class SagaEventStoreMock : ISagaEventStore
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
