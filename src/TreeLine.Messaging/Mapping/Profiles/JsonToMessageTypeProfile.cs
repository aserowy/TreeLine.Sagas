using AutoMapper;
using Newtonsoft.Json.Linq;

namespace TreeLine.Messaging.Mapping.Profiles
{
    internal class JsonToMessageTypeProfile : Profile
    {
        public JsonToMessageTypeProfile()
        {
            CreateMap<JObject, MessageTypeBase>()
                .ForMember(t => t.Type, exp => exp.MapFrom(s => s[nameof(MessageTypeBase.Type)]))
                .ForMember(t => t.Version, exp => exp.MapFrom(s => s[nameof(MessageTypeBase.Version)]))
                .ForMember(t => t.TargetType, exp => exp.MapFrom(s => s[nameof(MessageTypeBase.TargetType)]));
        }
    }
}