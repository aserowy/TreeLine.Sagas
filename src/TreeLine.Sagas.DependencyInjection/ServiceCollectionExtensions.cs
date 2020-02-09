using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Processor.Business;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSagas(this IServiceCollection services)
        {
            services
                .Configure()
                .AddEventStoreIfNotExists();

            return services;
        }

        public static IServiceCollection AddSagas(this IServiceCollection services, Action<IConfiguration> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var configuration = new Configuration();
            configure.Invoke(configuration);

            configuration.Configure(services);

            services
                .Configure()
                .AddEventStoreIfNotExists();

            return services;
        }

        [Obsolete("Use AddSagas(this IServiceCollection services, Action<IConfiguration> configure) to configure IEventStore instead.")]
        public static IServiceCollection AddSagas<TEventStore>(this IServiceCollection services) where TEventStore : class, ISagaEventStore
        {
            services
                .Configure()
                .AddTransient<ISagaEventStore, TEventStore>();

            return services;
        }

        internal static IServiceCollection Configure(this IServiceCollection services)
        {
            services.AddSingleton(services);

            services.AddTransient<ISagaFactory, SagaFactory>();

            services.AddTransient(typeof(ISaga<>), typeof(Saga<>));

            services.AddTransient<ISagaServiceProvider, SagaServiceProvider>();
            services.AddTransient<ISagaProcessorBuilder, SagaProcessorBuilder>();
            services.AddTransient<ISagaVersionBuilder, SagaVersionBuilder>();

            services.AddTransient<ISagaVersionFactory, SagaVersionFactory>();

            services.AddTransient<ISagaProcessor, SagaProcessor>();
            services.AddTransient<ISagaVersionStepResolver, SagaVersionStepResolver>();
            services.AddTransient<ISagaStepResolver, SagaStepResolver>();
            services.AddTransient<ISagaVersionResolver, SagaVersionResolver>();

            services.AddTransient<ISagaProcess, SagaProcess>();

            return services;
        }

        internal static IServiceCollection AddEventStoreIfNotExists(this IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(ISagaEventStore)))
            {
                services.AddTransient<ISagaEventStore, NullSagaEventStore>();
            }

            return services;
        }
    }
}