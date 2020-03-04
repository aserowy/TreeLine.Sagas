using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeLine.Messaging.Mapping
{
    internal static class MapperConfigurationExpressionExtensions
    {
        public static IEnumerable<Type> AddJObjectMapping(this IMapperConfigurationExpression expression, IMessageType type)
        {
            var map = expression
                .CreateMap(typeof(JObject), type.TargetType)
                .IncludeBase(typeof(JObject), typeof(MessageBase));

            var customClassTypes = new List<Type>();
            foreach (var member in type.TargetType.GetPublicMember())
            {
                map.RegisterJObjectMember(member.Name, member.Type);
                customClassTypes.AddTypeIfCustom(member.Type);
            }

            return customClassTypes;
        }

        public static void AddJTokenMapping(this IMapperConfigurationExpression expression, IEnumerable<Type> types)
        {
            var originTypes = types
                .Where(typ => typ.IsCustom())
                .ToList();

            var customTypes = new List<Type>(originTypes);
            foreach (var type in originTypes)
            {
                customTypes.AddTypeIfCustom(type.ResolveCustomTypes());
            }

            foreach (var type in customTypes.Distinct())
            {
                var map = expression.CreateMap(typeof(JToken), type);
                foreach (var member in type.GetPublicMember())
                {
                    map.RegisterJTokenMember(member.Name, member.Type);
                }
            }
        }

        private static void RegisterJObjectMember(this IMappingExpression map, string memberName, Type memberType)
        {
            map.ForMember(memberName, cnfgrtn => cnfgrtn.MapFrom((src, _) =>
            {
                if (!(src is JObject jObject))
                {
                    throw new AutoMapperMappingException($"{nameof(src)} is not of type {nameof(JObject)}.");
                }

                return jObject[memberName]?.ToObject(memberType) ?? JToken.Parse("{}");
            }));
        }

        private static void RegisterJTokenMember(this IMappingExpression map, string memberName, Type memberType)
        {
            map.ForMember(memberName, cnfgrtn => cnfgrtn.MapFrom((src, _) =>
            {
                if (!(src is JToken jToken))
                {
                    throw new AutoMapperMappingException($"{nameof(src)} is not of type {nameof(JToken)}.");
                }

                return jToken[memberName]?.ToObject(memberType) ?? JToken.Parse("{}");
            }));
        }

        private static IEnumerable<Type> ResolveCustomTypes(this Type type, IList<Type>? customTypes = null)
        {
            if (customTypes is null)
            {
                customTypes = new List<Type>();
            }
            else
            {
                if (customTypes.Contains(type))
                {
                    return customTypes;
                }
            }

            foreach (var member in type.GetPublicMember())
            {
                var isCustom = customTypes.AddTypeIfCustom(member.Type);
                if (isCustom)
                {
                    member.Type.ResolveCustomTypes(customTypes);
                }
            }

            return customTypes;
        }

        private static IEnumerable<(string Name, Type Type)> GetPublicMember(this Type type)
        {
            var properties = type.GetProperties();
            var settableProperties = properties.Where(prprty => prprty.CanWrite);

            var result = new List<(string Name, Type Type)>();
            foreach (var propertyInfo in settableProperties)
            {
                result.Add((propertyInfo.Name, propertyInfo.PropertyType));
            }

            var settableFields = type.GetFields();
            foreach (var fieldInfo in settableFields)
            {
                result.Add((fieldInfo.Name, fieldInfo.FieldType));
            }

            return result;
        }

        private static void AddTypeIfCustom(this IList<Type> customClassTypes, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (customClassTypes.Contains(type))
                {
                    continue;
                }

                customClassTypes.AddTypeIfCustom(type);
            }
        }

        private static bool AddTypeIfCustom(this IList<Type> customClassTypes, Type type)
        {
            var result = type.IsCustom();
            if (result)
            {
                customClassTypes.Add(type);
            }

            return result;
        }
    }
}
