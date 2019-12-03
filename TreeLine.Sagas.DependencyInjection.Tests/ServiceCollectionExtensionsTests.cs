using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;
using Xunit;

namespace TreeLine.Sagas.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSagas_ProfileAndStepConfigured_AllDependenciesResolved()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddSagas<SagaCommandSenderMock>()
                .AddTransient<SagaProfileMock>()
                .AddTransient<SagaStepMock>();

            // Act
            var saga = services
                .BuildServiceProvider()
                .GetRequiredService<ISagaFactory>()
                .Create<SagaProfileMock>();

            // Assert
            saga.RunAsync(new SagaEvent());
        }

        private sealed class SagaCommandSenderMock : ISagaCommandSender
        {
            public Task SendAsync(params ISagaCommand[] commands)
            {
                return Task.CompletedTask;
            }
        }

        private sealed class SagaProfileMock : ISagaProfile
        {
            public void Configure(ISagaProcessorBuilder processorBuilder)
            {
                processorBuilder.AddStep<SagaEvent, SagaStepMock>();
            }
        }

        private sealed class SagaStepMock : ISagaStep
        {
            public Task<IEnumerable<ISagaCommand>> RunAsync(ISagaEvent sagaEvent)
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