using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
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
                .AddTransient<SagaStepMock>();

            // Act
            var saga = services
                .BuildServiceProvider()
                .GetRequiredService<ISagaFactory>()
                .Create<SagaProfileMock>();

            var commands = await saga
                .RunAsync(new SagaEvent())
                .ConfigureAwait(false);

            // Assert
            Assert.NotEmpty(commands);
        }

        private sealed class SagaProfileMock : ISagaProfile
        {
            public void Configure(ISagaProcessorBuilder processorBuilder)
            {
                processorBuilder
                    .AddVersion("1.0.0")
                    .AddStep<SagaEvent, SagaStepMock>();
            }
        }

        private sealed class SagaStepMock : ISagaStep<SagaEvent>
        {
            public Task<IEnumerable<ISagaCommand>> RunAsync(SagaEvent sagaEvent)
            {
                return Task.FromResult(new[] { new SagaCommand() }.AsEnumerable<ISagaCommand>());
            }
        }

        private sealed class SagaCommand : ISagaCommand
        {
            public SagaCommand()
            {
                ReferenceId = Guid.NewGuid();
                TransactionId = Guid.NewGuid();
            }

            public Guid ReferenceId { get; set; }

            public Guid TransactionId { get; set; }
        }

        private sealed class SagaEvent : ISagaEvent
        {
            public SagaEvent()
            {
                ReferenceId = Guid.NewGuid();
                TransactionId = Guid.NewGuid();
            }

            public Guid ReferenceId { get; set; }

            public Guid TransactionId { get; set; }
        }
    }
}