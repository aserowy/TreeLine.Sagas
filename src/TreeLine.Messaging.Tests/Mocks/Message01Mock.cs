using System;

namespace TreeLine.Messaging.Tests.Mocks
{
    public sealed class Message01Mock : MessageBase
    {
        public Message01Mock() : base(new MessageType01Mock())
        {
        }

        public Guid Id { get; set; }
    }
}
