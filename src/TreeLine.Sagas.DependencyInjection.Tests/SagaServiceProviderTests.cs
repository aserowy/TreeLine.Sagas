using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class SagaServiceProviderTests
    {
        private SagaServiceProvider CreateProvider()
        {
            var services = new ServiceCollection();
            services
                .AddLogging()
                .AddSagas()
                .AddTransient<SagaProfileMock>()
                .AddTransient<SagaStep01Mock>();

            var provider = services.BuildServiceProvider();

            return new SagaServiceProvider(provider);
        }

        [Fact]
        public void Resolve_CalledTwice_ResolvedNotSameProcessorsSagaSteps()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var result01 = provider.Resolve<SagaEvent01, SagaStep01Mock>();
            var result02 = provider.Resolve<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.NotSame(result01, result02);
        }
    }
}
