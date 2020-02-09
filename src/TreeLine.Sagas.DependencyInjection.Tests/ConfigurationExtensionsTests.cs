using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using TreeLine.Sagas.EventStore;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void AddEventStore_SagaEventStoreMock_SagaEventStoreMockAdded()
        {
            // Arrange
            var configuration = new Configuration();
            var services = new ServiceCollection();

            // Act
            configuration.AddEventStore<SagaEventStoreMock>();
            configuration.Configure(services);

            // Assert
            var description = services.Single(x => x.ServiceType == typeof(ISagaEventStore));

            Assert.Equal(typeof(SagaEventStoreMock), description.ImplementationType);
        }

        [Fact]
        public void AddEventStore_CalledTwice_EventStoreAddedTwice()
        {
            // Arrange
            var configuration = new Configuration();
            var services = new ServiceCollection();

            // Act
            configuration.AddEventStore<SagaEventStoreMock>();
            configuration.AddEventStore<SagaEventStoreMock>();
            configuration.Configure(services);

            // Assert
            var descriptions = services.Where(x => x.ServiceType == typeof(ISagaEventStore));

            Assert.Equal(2, descriptions.Count());
        }
    }
}
