using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using TreeLine.Sagas.ReferenceStore;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void AddReferenceStore_SagaReferenceStoreMock_SagaReferenceStoreMockAdded()
        {
            // Arrange
            var configuration = new Configuration();
            var services = new ServiceCollection();

            // Act
            configuration.AddReferenceStore<SagaReferenceStoreMock>();
            configuration.Configure(services);

            // Assert
            var description = services.Single(x => x.ServiceType == typeof(IReferenceStore));

            Assert.Equal(typeof(SagaReferenceStoreMock), description.ImplementationType);
        }

        [Fact]
        public void AddReferenceStore_CalledTwice_ReferenceStoreAddedTwice()
        {
            // Arrange
            var configuration = new Configuration();
            var services = new ServiceCollection();

            // Act
            configuration.AddReferenceStore<SagaReferenceStoreMock>();
            configuration.AddReferenceStore<SagaReferenceStoreMock>();
            configuration.Configure(services);

            // Assert
            var descriptions = services.Where(x => x.ServiceType == typeof(IReferenceStore));

            Assert.Equal(2, descriptions.Count());
        }
    }
}
