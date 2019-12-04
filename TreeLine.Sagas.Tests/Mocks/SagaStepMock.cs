using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaStepMock : ISagaStep
    {
        public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent)
        {
            return Task.FromResult(new[] { new SagaCommand() }.AsEnumerable<ISagaCommand>());
        }
    }
}