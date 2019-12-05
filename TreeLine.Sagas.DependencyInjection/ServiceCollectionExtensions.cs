using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Processor;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSagas(this IServiceCollection services)
        {
            services.AddSingleton(services);

            services.AddTransient<ISagaFactory, SagaFactory>();

            services.AddTransient(typeof(ISaga<>), typeof(Saga<>));

            services.AddTransient<ISagaServiceProvider, SagaServiceProvider>();
            services.AddTransient<ISagaProcessorBuilder, SagaProcessorBuilder>();

            services.AddTransient<ISagaProcessor, SagaProcessor>();
            services.AddTransient<ISagaProcess, SagaProcess>();

            return services;
        }
    }
}