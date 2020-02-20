using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Processing;
using TreeLine.Sagas.Processing.Business;
using TreeLine.Sagas.ReferenceStore;
using TreeLine.Sagas.Validation;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSagas(this IServiceCollection services)
        {
            services
                .ConfigureSagas()
                .ConfigureValidation()
                .AddReferenceStoreIfNotExists();

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

            configuration
                .Configure(services)
                .AddSagas();

            return services;
        }

        internal static IServiceCollection ConfigureSagas(this IServiceCollection services)
        {
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            services.AddTransient<ISagaFactory, SagaFactory>();

            services.AddTransient(typeof(ISaga<>), typeof(Saga<>));

            services.AddTransient<ISagaServiceProvider, SagaServiceProvider>();
            services.AddTransient<ISagaProcessorBuilder, SagaProcessorBuilder>();

            services.AddTransient<ISagaVersionFactory, SagaVersionFactory>();

            services.AddTransient<ISagaProcessor, SagaProcessor>();
            services.AddTransient<ISagaVersionStepResolver, SagaVersionStepResolver>();
            services.AddTransient<ISagaStepResolver, SagaStepResolver>();
            services.AddTransient<ISagaVersionResolver, SagaVersionResolver>();

            services.AddTransient<ISagaProcess, SagaProcess>();

            return services;
        }

        internal static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator, Validator>();

            services.AddTransient<ISagaProfileAnalyzerFactory, SagaProfileAnalyzerFactory>();
            services.AddTransient<ISagaProfileAnalyzerResolver, SagaProfileAnalyzerResolver>();
            services.AddTransient<ISagaProfileVersionAnalyzerFactory, SagaProfileVersionAnalyzerFactory>();

            services.AddTransient<IValidationRule, MultipleVersionsWithEqualIdentifierRule>();
            services.AddTransient<IValidationRule, MultipleVersionsWithoutReferenceStoreRule>();
            services.AddTransient<IValidationRule, NoSagasRegisteredRule>();
            services.AddTransient<IValidationRule, VersionsWithoutStepsRule>();

            return services;
        }

        internal static IServiceCollection AddReferenceStoreIfNotExists(this IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(IReferenceStore)))
            {
                services.AddTransient<IReferenceStore, EmptyReferenceStore>();
            }

            return services;
        }
    }
}