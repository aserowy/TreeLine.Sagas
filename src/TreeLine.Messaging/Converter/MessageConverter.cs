using System;
using AutoMapper;

namespace TreeLine.Messaging.Converter
{
    public interface IMessageConverter
    {
        T ConvertTo<T>(IMessage message) where T : IMessage;
    }

    internal class MessageConverter : IMessageConverter
    {
        private readonly IMapper _mapper;

        public MessageConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public T ConvertTo<T>(IMessage message) where T : IMessage
        {
            try
            {
                if (message is T variable)
                {
                    return variable;
                }

                var result = _mapper.Map<T>(message);

                return result;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Conversion from message with type {message.Type?.Type} and version {message.Type?.Version} into {typeof(T).Name} was not possible.", e);
            }
        }
    }
}