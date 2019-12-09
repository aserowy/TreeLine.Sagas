using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Processor
{
    public class SagaProcessorTests : IDisposable
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaVersionStepResolver> _mockSagaVersionStepResolver;
        private readonly Mock<ISagaServiceProvider> _mockSagaServiceProvider;
        private readonly Mock<ISagaProcess> _mockSagaProcess;
        private readonly Mock<ILogger<SagaProcessor>> _mockLogger;

        public SagaProcessorTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaVersionStepResolver = _mockRepository.Create<ISagaVersionStepResolver>();
            _mockSagaServiceProvider = _mockRepository.Create<ISagaServiceProvider>();
            _mockSagaProcess = _mockRepository.Create<ISagaProcess>();
            _mockLogger = _mockRepository.Create<ILogger<SagaProcessor>>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaProcessor CreateSagaProcessor()
        {
            return new SagaProcessor(
                _mockSagaVersionStepResolver.Object,
                _mockSagaServiceProvider.Object,
                _mockSagaProcess.Object,
                _mockLogger.Object);
        }

        [Fact]
        public void AddSteps_ConfigurationsAreNull_ThrowArgumentNull()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            IList<ISagaStepConfiguration> configurations = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => sagaProcessor.AddSteps(new SagaVersion("1.0.0"), configurations));
        }

        [Fact]
        public async Task RunAsync_EventIsNull_ThrowArgumentNull()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            ISagaEvent sagaEvent = null;

            // Assert
            await Assert
                .ThrowsAsync<ArgumentNullException>(() => sagaProcessor.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_NoAddStepsCalled_ThrowInvalidOperation()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            var sagaEvent = new SagaEvent01();

            // Assert
            await Assert
                .ThrowsAsync<InvalidOperationException>(() => sagaProcessor.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }
    }
}
