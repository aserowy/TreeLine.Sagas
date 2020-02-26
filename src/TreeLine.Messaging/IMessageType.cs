using System;

namespace TreeLine.Messaging
{
    public interface IMessageType
    {
        Type TargetType { get; }
        string Type { get; }
        string Version { get; }
    }
}