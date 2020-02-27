using Newtonsoft.Json;
using System;

namespace TreeLine.Messaging
{
    public abstract class MessageTypeBase : IMessageType
    {
        protected MessageTypeBase(string type, string version, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Should not be null or empty.", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Should not be null or empty.", nameof(version));
            }

            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            Type = type;
            Version = version;
            TargetType = targetType;
        }

        [JsonProperty(Required = Required.Always)]
        public string Type { get; internal set; }

        [JsonProperty(Required = Required.Always)]
        public string Version { get; internal set; }

        [JsonProperty(Required = Required.Always)]
        public Type TargetType { get; internal set; }

        public override bool Equals(object obj)
        {
            if (!(obj is IMessageType message))
            {
                return false;
            }

            return Type.Equals(message.Type, StringComparison.InvariantCultureIgnoreCase)
                && Version.Equals(message.Version, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode(StringComparison.InvariantCultureIgnoreCase) ^ 17
                 * Version.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }
    }
}