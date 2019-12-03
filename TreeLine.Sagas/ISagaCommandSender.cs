using System.Threading.Tasks;

namespace TreeLine.Sagas
{
    public interface ISagaCommandSender
    {
        Task SendAsync(params ISagaCommand[] commands);
    }
}