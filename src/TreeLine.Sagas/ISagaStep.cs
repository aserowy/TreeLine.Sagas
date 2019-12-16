using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas
{
    public interface ISagaStep<TEvent> where TEvent : ISagaEvent
    {
        Task<IEnumerable<ISagaCommand>> RunAsync(TEvent sagaEvent);
    }
}