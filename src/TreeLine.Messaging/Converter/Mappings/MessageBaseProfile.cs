using AutoMapper;

namespace TreeLine.Messaging.Converter.Mappings
{
    internal class MessageBaseProfile : Profile
    {
        public MessageBaseProfile()
        {
            CreateMap<MessageBase, MessageBase>()
                .ForMember(t => t.Type, exp => exp.Ignore());
        }
    }
}