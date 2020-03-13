using Moq;
using System;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Building
{
    public class SagaVersionBuilderTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISagaVersion> _mockSagaVersion;

        public SagaVersionBuilderTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaVersion = _mockRepository.Create<ISagaVersion>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaVersionBuilder CreateSagaVersionBuilder()
        {
            return new SagaVersionBuilder(_mockSagaVersion.Object);
        }

        [Fact]
        public void Build_OneStepAdded_StepsContainsOne()
        {
            // Arrange
            var sagaVersionBuilder = CreateSagaVersionBuilder();
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.Equal(1, sagaVersionBuilder.Steps.Count);
        }

        [Fact]
        public void Build_TwoStepsAdded_StepsContainsTwo()
        {
            // Arrange
            var sagaVersionBuilder = CreateSagaVersionBuilder();
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.Equal(2, sagaVersionBuilder.Steps.Count);
        }

        [Fact]
        public void Build_VersionSet_VersionIsUsedInConfiguration()
        {
            // Arrange
            var sagaVersionBuilder = CreateSagaVersionBuilder();
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.Equal(_mockSagaVersion.Object, sagaVersionBuilder.Steps[0].Version);
        }
    }
}