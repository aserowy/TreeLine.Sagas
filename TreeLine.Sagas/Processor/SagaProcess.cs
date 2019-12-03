using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Processor
{
    internal interface ISagaProcess
    {
        Task RunAsync(ISagaEvent sagaEvent, ISagaStep step);
    }

    internal sealed class SagaProcess : ISagaProcess
    {
        private readonly ISagaCommandSender _sender;

        public SagaProcess(ISagaCommandSender sender)
        {
            _sender = sender;
        }

        public async Task RunAsync(ISagaEvent sagaEvent, ISagaStep step)
        {
            // TODO: Persist event

            var commands = await step
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // TODO: Persist commands

            await _sender
                .SendAsync(commands.ToArray())
                .ConfigureAwait(false);
        }
    }
}