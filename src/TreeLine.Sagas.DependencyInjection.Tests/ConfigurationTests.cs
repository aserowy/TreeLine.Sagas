using Microsoft.Extensions.DependencyInjection;
using System;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using TreeLine.Sagas.EventStore;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void Add_ActionIsNull_ThrowArgumentNull()
        {
            // Arrange
            var configuration = new Configuration();
            Action<IServiceCollection> action = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => configuration.Add(action));
        }

        [Fact]
        public void Configure_ServicesIsNull_ThrowArgumentNull()
        {
            // Arrange
            var configuration = new Configuration();
            IServiceCollection services = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => configuration.Configure(services));
        }

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
            configuration.Add(services => services.AddTransient<ISagaEventStore, SagaEventStoreMock>());
            configuration.Add(services => services.AddTransient<ISagaEventStore, SagaEventStoreMock>());
            configuration.Configure(services);

            // Assert
            Assert.NotEmpty(services);
        }
    }
}
