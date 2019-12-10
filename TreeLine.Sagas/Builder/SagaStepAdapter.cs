using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaStepAdapter
    {
        int Index { get; }

        public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent);
    }

    internal sealed class SagaStepAdapter<TEvent> : ISagaStepAdapter where TEvent : ISagaEvent
    {
        private readonly ISagaStep<TEvent> _step;

        public SagaStepAdapter(int index, ISagaStep<TEvent> step)
        {
            Index = index;

            _step = step ?? throw new ArgumentNullException(nameof(step));
        }

        public int Index { get; }

        public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent)
        {
            if (sagaEvent is null)
            {
                throw new ArgumentNullException(nameof(sagaEvent));
            }

            if (!(sagaEvent is TEvent converted))
            {
                throw new ArgumentOutOfRangeException($"{nameof(sagaEvent)} is not of type {typeof(TEvent).Name}.");
            }

            return _step.RunAsync(converted);
        }
    }
}