using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas
{
    public interface ISagaStep<TEvent> where TEvent : ISagaEvent
    {
        public Task<IEnumerable<ISagaCommand>> RunAsync(TEvent sagaEvent);
    }
}