using System;

namespace TreeLine.Messaging
{
    internal class MessageType : MessageTypeBase
    {
        public MessageType() : base("", "", typeof(Type))
        {
        }
    }
}