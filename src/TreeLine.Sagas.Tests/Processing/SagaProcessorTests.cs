using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Processing;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Processing
{
    public class SagaProcessorTests : IDisposable
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaVersionStepResolver> _mockSagaVersionStepResolver;
        private readonly Mock<ISagaServiceProvider> _mockSagaServiceProvider;
        private readonly Mock<ISagaProcess> _mockSagaProcess;
        private readonly Mock<ILoggerAdapter<SagaProcessor>> _mockLogger;

        public SagaProcessorTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaVersionStepResolver = _mockRepository.Create<ISagaVersionStepResolver>();
            _mockSagaServiceProvider = _mockRepository.Create<ISagaServiceProvider>();
            _mockSagaProcess = _mockRepository.Create<ISagaProcess>();
            _mockLogger = _mockRepository.Create<ILoggerAdapter<SagaProcessor>>();
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

        [Fact]
        public async Task RunAsync_ValidCall_ReturnsCommand()
        {
            // Arrange
            var sagaProcessor = CreateSagaProcessor();
            var sagaEvent = new SagaEvent01();
            var sagaVersion = new SagaVersion("1.0.0");

            var mockSagaStep = new SagaStepAdapter<SagaEvent01>(sagaVersion, 0, new SagaStep01Mock());
            var mockConfiguration01 = _mockRepository.Create<ISagaStepConfiguration>();
            mockConfiguration01
                .Setup(cnfgrtn => cnfgrtn.Create(It.IsAny<ISagaServiceProvider>()))
                .Returns(mockSagaStep);

            var mockConfiguration02 = _mockRepository.Create<ISagaStepConfiguration>();

            var configurations = new List<ISagaStepConfiguration>
            {
                mockConfiguration01.Object,
                mockConfiguration02.Object
            };

            _mockSagaVersionStepResolver
                .Setup(rslvr => rslvr.ResolveAsync(sagaEvent, It.Is<IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>>(dctnry => dctnry.Single().Value == configurations)))
                .ReturnsAsync(mockConfiguration01.Object);

            _mockSagaProcess
                .Setup(prcss => prcss.RunAsync(sagaEvent, mockSagaStep))
                .ReturnsAsync(await mockSagaStep.RunAsync(sagaEvent).ConfigureAwait(false));

            // Act
            sagaProcessor.AddSteps(sagaVersion, configurations);

            var commands = await sagaProcessor
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // Assert
            Assert.NotEmpty(commands);
        }
    }
}
