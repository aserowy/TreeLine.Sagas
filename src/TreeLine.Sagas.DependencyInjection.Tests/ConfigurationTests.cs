using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using TreeLine.Sagas.ReferenceStore;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void Configure_NoConfigurationsAdded_ServiceCollectionEmpty()
        {
            // Arrange
            var configuration = new Configuration();
            var services = new ServiceCollection();

            // Act
            configuration.Configure(services);

            // Assert
            Assert.Empty(services);
        }

        [Fact]
        public void Configure_MultipleConfigurationsAdded_ServiceCollectionContains()
        {
            // Arrange
            var configuration = new Configuration();
            var services = new ServiceCollection();

            // Act
            configuration.Add(services => services.AddTransient<IReferenceStore, SagaReferenceStoreMock>());
            configuration.Add(services => services.AddTransient<IReferenceStore, SagaReferenceStoreMock>());
            configuration.Configure(services);

            // Assert
            Assert.NotEmpty(services);
        }
    }
}
