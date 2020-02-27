using AutoMapper;
using Newtonsoft.Json.Linq;

namespace TreeLine.Messaging.Mapping.Profiles
{
    internal class JsonToMessageBaseProfile : Profile
    {
        public JsonToMessageBaseProfile()
        {
            CreateMap<JObject, MessageBase>()
                .ForMember(t => t.Type, exp => exp.Ignore())
                .ForMember(t => t.TimeOffset, exp => exp.MapFrom(s => s[nameof(MessageBase.TimeOffset)]));
        }
    }
}