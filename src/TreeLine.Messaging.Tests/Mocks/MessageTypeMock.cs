namespace TreeLine.Messaging.Tests.Mocks
{
    public sealed class MessageTypeMock : MessageTypeBase
    {
        public MessageTypeMock() : base("MessageMock", "1", typeof(MessageMock))
        {
        }
    }
}
