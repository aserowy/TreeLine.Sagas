using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Processing
{
    internal interface ISagaProcess
    {
        Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent, ISagaStepAdapter step);
    }

    internal sealed class SagaProcess : ISagaProcess
    {
        private readonly ISagaEventStore _store;

        public SagaProcess(ISagaEventStore store)
        {
            _store = store;
        }

        public async Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent, ISagaStepAdapter step)
        {
            var eventReference = new SagaReference(step.Version, step.Index, SagaMessageType.Event, sagaEvent.ProcessId, sagaEvent.TransactionId, sagaEvent);

            await _store
                .AddReferences(eventReference)
                .ConfigureAwait(false);

            var commands = await step
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            if (commands is null)
            {
                return Enumerable.Empty<ISagaCommand>();
            }

            var commandReferences = commands
                .Select(cmnd => new SagaReference(step.Version, step.Index, SagaMessageType.Command, cmnd.ProcessId, cmnd.TransactionId, cmnd))
                .ToArray();

            await _store
                .AddReferences(commandReferences)
                .ConfigureAwait(false);

            return commands;
        }
    }
}