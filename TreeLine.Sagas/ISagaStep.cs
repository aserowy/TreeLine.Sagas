using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeLine.Sagas
{
    public interface ISagaStep
    {
        public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent);
    }
}