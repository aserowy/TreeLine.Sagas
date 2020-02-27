using AutoMapper;
using Newtonsoft.Json.Linq;

namespace TreeLine.Messaging.Mapping
{
    internal static class MapperConfigurationExpressionExtensions
    {
        public static void AddMessageTypeMappings(this IMapperConfigurationExpression expression, IMessageType type)
        {
            var map = expression
                .CreateMap(typeof(JObject), type.TargetType)
                .IncludeBase(typeof(JObject), typeof(MessageBase));

            // TODO: Create map froms for properties
            // TODO: Create maps for complex types nested in target type
        }
    }
}
