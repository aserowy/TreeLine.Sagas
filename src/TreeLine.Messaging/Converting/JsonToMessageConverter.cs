using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using TreeLine.Messaging.Mapping;

namespace TreeLine.Messaging.Converting
{
    internal class JsonToMessageConverter : IConverter<string, IMessage>
    {
        private readonly IMessageTypeToClassResolver _classResolver;
        private readonly IMapperAdapter _mapper;
        private readonly IConverter<string, JObject> _stringToJObjectConverter;

        public JsonToMessageConverter(
            IConverter<string, JObject> stringToJObjectConverter,
            IMessageTypeToClassResolver classResolver,
            IMapperAdapter mapper)
        {
            _stringToJObjectConverter = stringToJObjectConverter;
            _classResolver = classResolver;
            _mapper = mapper;
        }

        public IMessage Convert(string input)
        {
            var jObject = _stringToJObjectConverter.Convert(input);

            return (IMessage)_mapper.Map(
                jObject,
                typeof(JObject),
                GetTypeByMessageType(jObject)
            );
        }

        private Type GetTypeByMessageType(JObject jObject)
        {
            var messageType = GetMessageType(jObject);
            var resolvedType = _classResolver.Get(messageType.Type, messageType.Version);
            if (resolvedType is null)
            {
                throw new InvalidOperationException($"Type could not get resolved for type {messageType.Type} and version {messageType.Version}");
            }

            return resolvedType;
        }

        private static IMessageType GetMessageType(JObject jObject)
        {
            if (!jObject.TryGetValue(nameof(IMessage.Type), out var typeJToken))
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            MessageType? messageType = null;
            try
            {
                messageType = typeJToken.ToObject<MessageType>();
            }
            catch (JsonSerializationException e)
            {
                throw new InvalidCastException($"{nameof(MessageType)} could not get intantiated by object {jObject} with error '{e.InnerException.Message}'.");
            }

            if (messageType is null)
            {
                throw new InvalidCastException($"{nameof(MessageType)} could not get intantiated by object {jObject}.");
            }

            return messageType;
        }
    }
}