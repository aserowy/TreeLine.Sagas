using System;

namespace TreeLine.Messaging.Typing
{
    public partial class MessageType : EnumerationBase
    {
        public MessageType(string name) : base(name?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0, name ?? "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
        }
    }
}
