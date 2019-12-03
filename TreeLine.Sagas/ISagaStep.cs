using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas
{
    public interface ISagaStep
    {
        public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent);
    }
}