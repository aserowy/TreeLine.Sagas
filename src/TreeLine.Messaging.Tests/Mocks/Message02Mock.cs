namespace TreeLine.Messaging.Tests.Mocks
{
    public sealed class Message02Mock : MessageBase
    {
        public Message02Mock() : base(new MessageType02Mock())
        {
            Custom = new CustomClass();
        }

        public int Id { get; set; }
        public CustomClass Custom { get; set; }
    }

    public sealed class CustomClass
    {
        public CustomClass()
        {
            Name = "";
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
