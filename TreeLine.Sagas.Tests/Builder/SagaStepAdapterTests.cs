using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder
{
    public class SagaStepAdapterTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISagaStep<SagaEvent01>> _mockSagaStep;

        public SagaStepAdapterTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaStep = _mockRepository.Create<ISagaStep<SagaEvent01>>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaStepAdapter<SagaEvent01> CreateSagaStepAdapter()
        {
            return new SagaStepAdapter<SagaEvent01>(new SagaVersion("1.0.0"), 0, _mockSagaStep.Object);
        }

        [Fact]
        public void Ctor_SagaStepIsNull_ThrowArgumentNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SagaStepAdapter<SagaEvent01>(new SagaVersion("1.0.0"), 0, null));
        }

        [Fact]
        public void Ctor_VersionIsNull_ThrowArgumentNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SagaStepAdapter<SagaEvent01>(null, 0, new SagaStep01Mock()));
        }

        [Fact]
        public async Task RunAsync_EventIsNotOfGivenType_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var sagaStepAdapter = CreateSagaStepAdapter();
            var sagaEvent = new SagaEvent02();

            // Assert
            await Assert
                .ThrowsAsync<ArgumentOutOfRangeException>(() => sagaStepAdapter.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_EventIsNull_ThrowsArgumentNull()
        {
            // Arrange
            var sagaStepAdapter = CreateSagaStepAdapter();
            SagaEvent01 sagaEvent = null;

            // Assert
            await Assert
                .ThrowsAsync<ArgumentNullException>(() => sagaStepAdapter.RunAsync(sagaEvent))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task RunAsync_ValidInjections_CallsRunAsyncOfStep()
        {
            // Arrange
            var sagaStepAdapter = CreateSagaStepAdapter();
            var sagaEvent = new SagaEvent01();

            _mockSagaStep
                .Setup(stp => stp.RunAsync(It.IsAny<SagaEvent01>()))
                .ReturnsAsync(new List<ISagaCommand>());

            // Act
            await sagaStepAdapter
                .RunAsync(sagaEvent)
                .ConfigureAwait(false);

            // Assert
            _mockSagaStep.Verify(stp => stp.RunAsync(sagaEvent), Times.Once);
        }
    }
}