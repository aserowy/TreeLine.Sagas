using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Processor;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSagas<TSender>(this IServiceCollection services) where TSender : class, ISagaCommandSender
        {
            services.AddTransient<ISagaCommandSender, TSender>();

            services.AddTransient<ISagaFactory, SagaFactory>();

            services.AddTransient<ISagaServiceProvider, SagaServiceProvider>();

            services.AddTransient<ISagaProcessor, SagaProcessor>();

            return services;
        }
    }
}