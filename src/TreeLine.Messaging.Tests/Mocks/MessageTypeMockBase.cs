namespace TreeLine.Messaging.Tests.Mocks
{
    public abstract class MessageTypeMockBase : MessageTypeBase
    {
        public MessageTypeMockBase() : base("MessageMock", "1", typeof(Message01Mock))
        {
        }
    }
}
