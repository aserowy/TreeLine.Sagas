using AutoMapper;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TreeLine.Messaging.Mapping
{
    internal static class MapperConfigurationExpressionExtensions
    {
        public static void AddMessageTypeMappings(this IMapperConfigurationExpression expression, IMessageType type)
        {
            var map = expression
                .CreateMap(typeof(JObject), type.TargetType)
                .IncludeBase(typeof(JObject), typeof(MessageBase));

            var settableProperties = type
                .TargetType
                .GetProperties()
                .Where(prprty => prprty.CanWrite);

            //var customClassTypes = new List<Type>();
            foreach (var propertyInfo in settableProperties)
            {
                RegisterForMember(ref map, propertyInfo.Name);

                //if (!propertyInfo.PropertyType.Namespace.StartsWith("System", StringComparison.OrdinalIgnoreCase))
                //{
                //    customClassTypes.Add(propertyInfo.PropertyType);
                //}
            }

            var settableFields = type
                .TargetType
                .GetFields();

            foreach (var fieldInfo in settableFields)
            {
                RegisterForMember(ref map, fieldInfo.Name);
            }

            // TODO: Create map froms for properties
            // TODO: Create maps for complex types nested in target type
        }

        private static void RegisterForMember(ref IMappingExpression map, string memberName)
        {
            map.ForMember(memberName, cnfgrtn => cnfgrtn.MapFrom((src, _) =>
            {
                if (!(src is JObject jObject))
                {
                    throw new AutoMapperMappingException($"{nameof(src)} is not of type {nameof(JObject)}.");
                }

                return jObject[memberName] ?? JToken.Parse("{}");
            }));
        }
    }
}
