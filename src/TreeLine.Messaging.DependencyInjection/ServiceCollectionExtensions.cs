using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TreeLine.Messaging.Converting;
using TreeLine.Messaging.Mapping;

namespace TreeLine.Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddTransient<IMessageFactory, MessageFactory>();

            services.AddSingleton<IMessageTypeResolver, MessageTypeResolver>();

            // Converting
            services.AddTransient<IConverter<DynamicWrapper, IMessage>, DynamicToMessageConverter>();
            services.AddTransient<IConverter<string, IMessage>, JsonToMessageConverter>();
            services.AddTransient<IConverter<string, JObject>, StringToJObjectConverter>();

            services.AddTransient<IMessageTypeToClassResolver, MessageTypeToClassResolver>();

            // Mapping
            services.AddTransient<IMapperAdapter, MapperAdapter>();

            services.AddSingleton<IMapperConfigurationProvider, MapperConfigurationProvider>();

            return services;
        }
    }
}
