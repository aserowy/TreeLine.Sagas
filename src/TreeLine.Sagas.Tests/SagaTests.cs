using Moq;
using System;
using System.Threading.Tasks;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processing;
using TreeLine.Sagas.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.Tests
{
    public class SagaTests : IDisposable
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaProfile> _mockSagaProfile;
        private readonly Mock<ISagaProcessorBuilder> _mockSagaProcessorBuilder;
        private readonly Mock<ISagaProcessor> _mockSagaProcessor;

        public SagaTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaProfile = _mockRepository.Create<ISagaProfile>();
            _mockSagaProcessorBuilder = _mockRepository.Create<ISagaProcessorBuilder>();
            _mockSagaProcessor = _mockRepository.Create<ISagaProcessor>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private Saga<ISagaProfile> CreateSaga()
        {
            return new Saga<ISagaProfile>(
                _mockSagaProfile.Object,
                _mockSagaProcessorBuilder.Object);
        }

        [Fact]
        public async Task RunAsync_EventIsNull_TrowArgumentNull()
        {
            // Arrange
            var saga = CreateSaga();
            ISagaEvent? sagaEvent = null;

            // Assert
            await Assert
                .ThrowsAsync<ArgumentNullException>(() => saga.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_RunWithEvent_CompleteRun()
        {
            // Arrange
            var saga = CreateSaga();
            var sagaEvent = new SagaEvent01();

            _mockSagaProfile.Setup(prfl => prfl.Configure(It.IsAny<ISagaProcessorBuilder>()));
            _mockSagaProcessorBuilder.Setup(bldr => bldr.Build()).Returns(_mockSagaProcessor.Object);
            _mockSagaProcessor.Setup(prcssr => prcssr.RunAsync(sagaEvent)).ReturnsAsync(new ArraySegment<ISagaCommand>());

            // Act
            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_RunTwice_ConfigureCalledOnce()
        {
            // Arrange
            var saga = CreateSaga();
            var sagaEvent = new SagaEvent01();

            _mockSagaProfile.Setup(prfl => prfl.Configure(It.IsAny<ISagaProcessorBuilder>()));
            _mockSagaProcessorBuilder.Setup(bldr => bldr.Build()).Returns(_mockSagaProcessor.Object);
            _mockSagaProcessor.Setup(prcssr => prcssr.RunAsync(sagaEvent)).ReturnsAsync(new ArraySegment<ISagaCommand>());

            // Act
            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // Assert
            _mockSagaProfile.Verify(prfl => prfl.Configure(It.IsAny<ISagaProcessorBuilder>()), Times.Once);
        }

        [Fact]
        public async Task RunAsync_RunTwice_BuildCalledOnce()
        {
            // Arrange
            var saga = CreateSaga();
            var sagaEvent = new SagaEvent01();

            _mockSagaProfile.Setup(prfl => prfl.Configure(It.IsAny<ISagaProcessorBuilder>()));
            _mockSagaProcessorBuilder.Setup(bldr => bldr.Build()).Returns(_mockSagaProcessor.Object);
            _mockSagaProcessor.Setup(prcssr => prcssr.RunAsync(sagaEvent)).ReturnsAsync(new ArraySegment<ISagaCommand>());

            // Act
            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // Assert
            _mockSagaProcessorBuilder.Verify(bldr => bldr.Build(), Times.Once);
        }

        [Fact]
        public async Task RunAsync_RunTwice_RunAsyncCalledTwice()
        {
            // Arrange
            var saga = CreateSaga();
            var sagaEvent = new SagaEvent01();

            _mockSagaProfile.Setup(prfl => prfl.Configure(It.IsAny<ISagaProcessorBuilder>()));
            _mockSagaProcessorBuilder.Setup(bldr => bldr.Build()).Returns(_mockSagaProcessor.Object);
            _mockSagaProcessor.Setup(prcssr => prcssr.RunAsync(sagaEvent)).ReturnsAsync(new ArraySegment<ISagaCommand>());

            // Act
            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            await saga
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // Assert
            _mockSagaProcessor.Verify(prcssr => prcssr.RunAsync(sagaEvent), Times.Exactly(2));
        }
    }
}