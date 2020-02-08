using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TreeLine.Sagas.DependencyInjection.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public async Task AddSagas_ProfileAndStepConfigured_AllDependenciesResolved()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddSagas()
                .AddLogging()
                .AddTransient<SagaProfileMock>()
                .AddTransient<SagaStep01Mock>();

            // Act
            var saga = services
                .BuildServiceProvider()
                .GetRequiredService<ISagaFactory>()
                .Create<SagaProfileMock>();

            var commands = await saga
                .RunAsync(new SagaEvent01())
                .ConfigureAwait(false);

            // Assert
            Assert.NotEmpty(commands);
        }
    }
}