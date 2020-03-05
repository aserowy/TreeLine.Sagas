using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TreeLine.Messaging.Converting
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
            if (resolvedType is null)
            {
                throw new InvalidOperationException($"Type could not get resolved for MessageType {type} and version {version}.");
            }

            return resolvedType;
        }

        private static (string Type, string Version) GetMessageType(DynamicWrapper wrapper)
        {
            var data = wrapper.Data;
            var messageType = ResolveValue(data, nameof(IMessage.Type));
            if (messageType is null)
            {
                throw new ArgumentNullException(nameof(wrapper));
            }

            var type = ResolveString(messageType, nameof(IMessageType.Type));
            var version = ResolveString(messageType, nameof(IMessageType.Version));

            return (type, version);
        }

        private static string ResolveString(dynamic obj, string propertyName)
        {
            var value = ResolveValue(obj, propertyName);
            if (value is null || !(value is string result))
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return result;
        }

        private static object ResolveValue(dynamic obj, string propertyName)
        {
            if (obj is IDictionary<string, object> expando)
            {
                if (!expando.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                return expando[propertyName];
            }
            else
            {
                var property = obj.GetType().GetProperty(propertyName);
                if (property is null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                return property.GetValue(obj);
            }
        }
    }
}