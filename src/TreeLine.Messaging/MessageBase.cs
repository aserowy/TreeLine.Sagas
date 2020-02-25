using Newtonsoft.Json;
using System;

namespace TreeLine.Messaging
{
    public interface IMessage
    {
        DateTimeOffset TimeOffset { get; }
        IMessageType Type { get; }

        string ToJson();
    }

    public abstract class MessageBase : IMessage
    {
        protected MessageBase(IMessageType type)
        {
            Type = type;
            TimeOffset = DateTimeOffset.Now;
        }

        [JsonProperty(Required = Required.Always)]
        public DateTimeOffset TimeOffset { get; }

        [JsonProperty(Required = Required.Always)]
        public IMessageType Type { get; }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}