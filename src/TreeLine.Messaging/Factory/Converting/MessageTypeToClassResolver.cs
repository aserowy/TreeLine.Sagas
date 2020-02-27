using System;
using System.Linq;

namespace TreeLine.Messaging.Factory.Converting
{
    internal interface IMessageTypeToClassResolver
    {
        Type Get(string type, string version);
    }

    internal class MessageTypeToClassResolver : IMessageTypeToClassResolver
    {
        private readonly IMessageTypeResolver _resolver;

        public MessageTypeToClassResolver(IMessageTypeResolver resolver)
        {
            _resolver = resolver;
        }

        public Type Get(string type, string version)
        {
            return _resolver
                .Get()
                .Single(msgtyp => msgtyp.Type == type && msgtyp.Version == version)
                .TargetType;
        }
    }
}