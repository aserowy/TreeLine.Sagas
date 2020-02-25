//using Microsoft.Extensions.DependencyInjection;
//using TreeLine.Messaging;
//using TreeLine.Messaging.Converter;
//using TreeLine.Messaging.Factory;
//using TreeLine.Messaging.Mapper;
//using Newtonsoft.Json.Linq;

//namespace SesamX.Messaging.Dependency
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection AddMessaging(this IServiceCollection services)
//        {
//            services.AddTransient<IMessageFactory, MessageFactory>();
//            services.AddTransient<IConverter<DynamicWrapper, IMessage>, DynamicToMessageConverter>();
//            services.AddTransient<IConverter<string, IMessage>, JsonToMessageConverter>();
//            services.AddTransient<IConverter<string, JObject>, StringToJObjectConverter>();

//            services.AddTransient<IMessageConverter, MessageConverter>();

//            services.AddSingleton<IMessageTypeToClassResolver, MessageTypeToClassResolver>();

//            return services;
//        }
//    }
//}