using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ServiceProviderExtensionsTests
    {
        [Fact]
        public void ValidateSagas_ProfileAndStepConfigured_AllDependenciesResolved()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddSagas()
                .AddLogging()
                .AddTransient<SagaProfileMock>()
                .AddTransient<SagaStep01Mock>();

            // Act
            var provider = services.BuildServiceProvider();

            // Assert
            provider.ValidateSagas();
        }
    }
}
