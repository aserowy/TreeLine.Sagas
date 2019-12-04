using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.Tests.Processor
{
    public class SagaProcessorTests : IDisposable
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaServiceProvider> _mockSagaServiceProvider;
        private readonly Mock<ISagaProcess> _mockSagaProcess;

        public SagaProcessorTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaServiceProvider = _mockRepository.Create<ISagaServiceProvider>();
            _mockSagaProcess = _mockRepository.Create<ISagaProcess>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaProcessor CreateSagaProcessor()
        {
            return new SagaProcessor(
                _mockSagaServiceProvider.Object,
                _mockSagaProcess.Object);
        }

        [Fact]
        public void AddSteps_ConfigurationsAreNull_ThrowArgumentNull()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            IList<ISagaStepConfiguration> configurations = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => sagaProcessor.AddSteps(configurations));
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
        public async Task RunAsync_MultipleIsResponsibleTrue_OnlyFirstGotCalled()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            var sagaEvent = new SagaEvent();

            _mockSagaProcess
                .Setup(prcss => prcss.RunAsync(It.IsAny<ISagaEvent>(), It.IsAny<ISagaStep>()))
                .Returns(Task.CompletedTask);

            var mockConfiguration01 = _mockRepository.Create<ISagaStepConfiguration>();
            mockConfiguration01
                .Setup(cnfg => cnfg.IsResponsible(It.IsAny<ISagaEvent>()))
                .Returns(true);

            mockConfiguration01
                .Setup(cnfg => cnfg.Create(It.IsAny<ISagaServiceProvider>()))
                .Returns(new SagaStepMock());

            var mockConfiguration02 = _mockRepository.Create<ISagaStepConfiguration>();

            sagaProcessor.AddSteps(new List<ISagaStepConfiguration>
            {
                mockConfiguration01.Object,
                mockConfiguration02.Object
            });

            // Act
            await sagaProcessor
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // Assert
            mockConfiguration01.Verify(cnfg => cnfg.IsResponsible(It.IsAny<ISagaEvent>()), Times.Once);
        }

        [Fact]
        public async Task RunAsync_AllIsResponsibleFalse_ThrowArgumentOutOfRange()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            var sagaEvent = new SagaEvent();

            var mockConfiguration01 = _mockRepository.Create<ISagaStepConfiguration>();
            mockConfiguration01
                .Setup(cnfg => cnfg.IsResponsible(It.IsAny<ISagaEvent>()))
                .Returns(false);

            var mockConfiguration02 = _mockRepository.Create<ISagaStepConfiguration>();
            mockConfiguration02
                .Setup(cnfg => cnfg.IsResponsible(It.IsAny<ISagaEvent>()))
                .Returns(false);

            sagaProcessor.AddSteps(new List<ISagaStepConfiguration>
            {
                mockConfiguration01.Object,
                mockConfiguration02.Object
            });

            // Assert
            await Assert
                .ThrowsAsync<ArgumentOutOfRangeException>(() => sagaProcessor.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_NoAddStepsCalled_ThrowInvalidOperation()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            var sagaEvent = new SagaEvent();

            // Assert
            await Assert
                .ThrowsAsync<InvalidOperationException>(() => sagaProcessor.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_EmptyListAddedWithAddSteps_ThrowInvalidOperation()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            var sagaEvent = new SagaEvent();

            sagaProcessor.AddSteps(new List<ISagaStepConfiguration>());

            // Assert
            await Assert
                .ThrowsAsync<InvalidOperationException>(() => sagaProcessor.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }
    }
}