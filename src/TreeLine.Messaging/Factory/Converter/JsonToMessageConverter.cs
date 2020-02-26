using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using TreeLine.Messaging.Mapper;

namespace TreeLine.Messaging.Factory.Converter
{
    internal class JsonToMessageConverter : IConverter<string, IMessage>
    {
        private readonly IMessageTypeToClassResolver _classResolver;
        private readonly IMapper _mapper;
        private readonly IConverter<string, JObject> _stringToJObjectConverter;

        public JsonToMessageConverter(IConverter<string, JObject> stringToJObjectConverter,
            IMessageTypeToClassResolver classResolver,
            IMapper mapper
        )
        {
            _stringToJObjectConverter = stringToJObjectConverter;
            _classResolver = classResolver;
            _mapper = mapper;
        }

        public IMessage Convert(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

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
            if (resolvedType == null)
            {
                throw new InvalidOperationException($"Type could not get resolved for type {messageType.Type} and version {messageType.Version}");
            }

            return resolvedType;
        }

        private IMessageType GetMessageType(JObject jObject)
        {
            if (!jObject.TryGetValue(nameof(IMessage.Type), out var typeJToken))
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            var messageType = typeJToken.ToObject<MessageType>();
            if (messageType is null)
            {
                throw new InvalidCastException($"{nameof(MessageType)} could not get intantiated by object {jObject}.");
            }

            return messageType;
        }
    }
}