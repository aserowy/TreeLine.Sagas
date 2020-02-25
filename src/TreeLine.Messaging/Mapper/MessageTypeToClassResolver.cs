using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeLine.Messaging.Mapper
{
    internal interface IMessageTypeToClassResolver
    {
        Type Get(string type, string version);
    }

    internal class MessageTypeToClassResolver : IMessageTypeToClassResolver
    {
        private readonly IEnumerable<IMessageType> _messageTypes;

        public MessageTypeToClassResolver(IEnumerable<IMessageType> messageTypes)
        {
            _messageTypes = messageTypes;
        }

        public Type Get(string type, string version)
        {
            return _messageTypes
                .Single(msgtyp => msgtyp.Type == type && msgtyp.Version == version)
                .TargetType;
        }
    }
}