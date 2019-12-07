using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaStep01Mock : ISagaStep<SagaEvent>
    {
        public Task<IEnumerable<ISagaCommand>> RunAsync(SagaEvent sagaEvent)
        {
            return Task.FromResult(new[] { new SagaCommand() }.AsEnumerable<ISagaCommand>());
        }
    }

    internal sealed class SagaStep02Mock : ISagaStep<SagaEvent>
    {
        public Task<IEnumerable<ISagaCommand>> RunAsync(SagaEvent sagaEvent)
        {
            return Task.FromResult(new[] { new SagaCommand() }.AsEnumerable<ISagaCommand>());
        }
    }
}