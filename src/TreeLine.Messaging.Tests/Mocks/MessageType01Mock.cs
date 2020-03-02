namespace TreeLine.Messaging.Tests.Mocks
{
    public sealed class MessageType01Mock : MessageTypeBase
    {
        public MessageType01Mock() : base("MessageMock", "1", typeof(Message01Mock))
        {
        }
    }
}
