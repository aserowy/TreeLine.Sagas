using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Processor
{
    internal interface ISagaProcess
    {
        Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent, ISagaStepAdapter step);
    }

    internal sealed class SagaProcess : ISagaProcess
    {
        public async Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent, ISagaStepAdapter step)
        {
            // TODO: Persist event

            var commands = await step
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // TODO: Validate commands (referenceId not null not empty)

            // TODO: Persist commands

            return commands;
        }
    }
}