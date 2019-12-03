using System.Threading.Tasks;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Processor
{
    public interface ISagaCommandSender
    {
        Task SendAsync(params ISagaCommand[] commands);
    }
}