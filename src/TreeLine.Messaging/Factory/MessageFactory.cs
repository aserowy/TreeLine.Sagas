namespace TreeLine.Messaging.Factory
{
    public interface IMessageFactory
    {
        IMessage Create(string json);

        IMessage Create(dynamic obj);
    }

    internal class MessageFactory : IMessageFactory
    {
        private readonly IConverter<DynamicWrapper, IMessage> _dynamicToMessageConverter;
        private readonly IConverter<string, IMessage> _jsonToMessageConverter;

        public MessageFactory(
            IConverter<string, IMessage> jsonToMessageConverter,
            IConverter<DynamicWrapper, IMessage> dynamicToMessageConverter)
        {
            _jsonToMessageConverter = jsonToMessageConverter;
            _dynamicToMessageConverter = dynamicToMessageConverter;
        }

        public IMessage Create(string json)
        {
            return _jsonToMessageConverter.Convert(json);
        }

        public IMessage Create(dynamic obj)
        {
            return _dynamicToMessageConverter.Convert(new DynamicWrapper(obj));
        }
    }
}