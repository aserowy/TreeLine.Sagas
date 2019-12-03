using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreeLine.Sagas
{
    public interface ISagaProcessor
    {
        void Add(IEnumerable<Type> events, ISagaStep sagaStep);
        Task RunAsync(ISagaEvent sagaEvent);
    }

    internal sealed class SagaProcessor : ISagaProcessor
    {
        private readonly ISagaProcess _process;
        private readonly IList<(IEnumerable<Type> Trigger, ISagaStep Step)> _steps;

        public SagaProcessor(ISagaProcess process)
        {
            _process = process;

            _steps = new List<(IEnumerable<Type> Trigger, ISagaStep Step)>();
        }

        public void Add(IEnumerable<Type> events, ISagaStep sagaStep)
        {
            _steps.Add((events, sagaStep));
        }

        public Task RunAsync(ISagaEvent sagaEvent)
        {
            var step = ResolveStep(sagaEvent);

            return _process.RunAsync(sagaEvent, step);
        }

        private ISagaStep ResolveStep(ISagaEvent sagaEvent)
        {
            foreach (var (trigger, step) in _steps)
            {
                if (trigger.Any(tp => tp.Equals(sagaEvent.GetType())))
                {
                    return step;
                }
            }

            throw new ArgumentOutOfRangeException($"No step for event type {sagaEvent.GetType().Name} found.");
        }
    }
}