using Newtonsoft.Json;
using System;
using TreeLine.Messaging.Mapper;

namespace TreeLine.Messaging.Factory
{
    internal class DynamicToMessageConverter : IConverter<DynamicWrapper, IMessage>
    {
        private readonly IMessageTypeToClassResolver _classResolver;

        public DynamicToMessageConverter(IMessageTypeToClassResolver classResolver)
        {
            _classResolver = classResolver;
        }

        public IMessage Convert(DynamicWrapper wrapper)
        {
            if (wrapper is null || wrapper.Data is null)
            {
                throw new ArgumentNullException(nameof(wrapper));
            }

            var type = GetTypeByMessageType(wrapper);

            var settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset };
            var representation = JsonConvert.SerializeObject(wrapper.Data, settings);
            var result = JsonConvert.DeserializeObject(representation, type, settings);

            return result;
        }

        private Type GetTypeByMessageType(DynamicWrapper wrapper)
        {
            var (type, version) = GetMessageType(wrapper);

            var resolvedType = _classResolver.Get(type, version);
            if (resolvedType == null)
            {
                throw new InvalidOperationException($"Type could not get resolved for MessageType {type} and version {version}.");
            }

            return resolvedType;
        }

        private static (string Type, string Version) GetMessageType(DynamicWrapper wrapper)
        {
            var data = wrapper.Data;
            if (data?.Type is null)
            {
                throw new ArgumentNullException(nameof(wrapper));
            }

            if (!(data.Type.Type is string type))
            {
                throw new ArgumentNullException(nameof(wrapper));
            }

            if (!(data.Type.Version is string version))
            {
                throw new ArgumentNullException(nameof(wrapper));
            }

            return new ValueTuple<string, string>(type, version);
        }
    }
}