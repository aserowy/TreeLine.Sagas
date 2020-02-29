namespace TreeLine.Messaging.Tests.Mocks
{
    public sealed class MessageMock : MessageBase
    {
        public MessageMock() : base(new MessageTypeMock())
        {
        }
    }
}
