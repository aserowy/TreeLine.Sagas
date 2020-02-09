using Microsoft.Extensions.DependencyInjection;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class SagaFactoryTests
    {
        private SagaFactory CreateFactory()
        {
            var services = new ServiceCollection();
            services
                .AddLogging()
                .AddSagas()
                .AddTransient<SagaProfileMock>()
                .AddTransient<SagaStep01Mock>();

            return new SagaFactory(services);
        }

        [Fact]
        public void Create_CalledTwice_ResolvedNotSameSaga()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result01 = factory.Create<SagaProfileMock>();
            var result02 = factory.Create<SagaProfileMock>();

            // Assert
            Assert.NotSame(result01, result02);
        }
    }
}
