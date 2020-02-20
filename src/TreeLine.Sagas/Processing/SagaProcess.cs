using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.ReferenceStore;

namespace TreeLine.Sagas.Processing
{
    internal interface ISagaProcess
    {
        Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent, ISagaStepAdapter step);
    }

    internal sealed class SagaProcess : ISagaProcess
    {
        private readonly IReferenceStore _store;

        public SagaProcess(IReferenceStore store)
        {
            _store = store;
        }

        public async Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent, ISagaStepAdapter step)
        {
            var eventReference = new SagaReference(step.Version, step.Index, SagaMessageType.Event, sagaEvent.ProcessId, sagaEvent.TransactionId);

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
                .Select(cmnd => new SagaReference(step.Version, step.Index, SagaMessageType.Command, cmnd.ProcessId, cmnd.TransactionId))
                .ToArray();

            await _store
                .AddReferences(commandReferences)
                .ConfigureAwait(false);

            return commands;
        }
    }
}