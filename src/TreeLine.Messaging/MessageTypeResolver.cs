using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TreeLine.Messaging
{
    internal interface IMessageTypeResolver
    {
        IList<IMessageType> Get();
    }

    internal sealed class MessageTypeResolver : IMessageTypeResolver
    {
        private readonly object _lock = new object();
        private readonly Assembly[] _assemblies;

        private IList<IMessageType>? _messageTypes;

        public MessageTypeResolver(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public IList<IMessageType> Get()
        {
            if (_messageTypes is null)
            {
                lock (_lock)
                {
                    if (_messageTypes is null)
                    {
                        _messageTypes = GenerateMessageTypes();
                    }
                }
            }

            return _messageTypes;
        }

        private IList<IMessageType> GenerateMessageTypes()
        {
            var scannedTypes = _assemblies
                .Where(assmbly => !assmbly.IsDynamic && assmbly != GetType().Assembly)
                .SelectMany(assmbly => assmbly.DefinedTypes)
                .Where(type => !type.IsAbstract && typeof(IMessageType).IsAssignableFrom(type));

            var result = new List<IMessageType>();
            foreach (var type in scannedTypes)
            {
                result.Add((IMessageType)Activator.CreateInstance(type));
            }

            return result;
        }
    }
}
