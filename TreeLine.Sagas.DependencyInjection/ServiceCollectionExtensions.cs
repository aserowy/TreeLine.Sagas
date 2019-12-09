using Microsoft.Extensions.DependencyInjection;
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
            services.Configure();
            services.AddTransient<ISagaEventStore, NullSagaEventStore>();

            return services;
        }

        public static IServiceCollection AddSagas<TEventStore>(this IServiceCollection services) where TEventStore : class, ISagaEventStore
        {
            services.Configure();
            services.AddTransient<ISagaEventStore, TEventStore>();

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
            services.AddTransient<ISagaVersionedStepResolver, SagaVersionedStepResolver>();
            services.AddTransient<ISagaStepResolver, SagaStepResolver>();
            services.AddTransient<ISagaVersionResolver, SagaVersionResolver>();

            services.AddTransient<ISagaProcess, SagaProcess>();

            return services;
        }
    }
}