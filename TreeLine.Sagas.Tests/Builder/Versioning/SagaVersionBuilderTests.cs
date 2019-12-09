using System;
using TreeLine.Sagas.Builder.Versioning;
using TreeLine.Sagas.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder.Versioning
{
    public class SagaVersionBuilderTests
    {
        [Fact]
        public void Ctor_VersionIsNull_ThrowArgumentNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SagaVersionBuilder(null));
        }

        [Fact]
        public void Build_OneStepAdded_StepsContainsOne()
        {
            // Arrange
            var sagaVersionBuilder = new SagaVersionBuilder("1");
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.Equal(1, sagaVersionBuilder.Steps.Count);
        }

        [Fact]
        public void Build_TwoStepsAdded_StepsContainsTwo()
        {
            // Arrange
            var sagaVersionBuilder = new SagaVersionBuilder("1");
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.Equal(2, sagaVersionBuilder.Steps.Count);
        }

        [Fact]
        public void Build_VersionSet_VersionIsUsedInConfiguration()
        {
            // Arrange
            var sagaVersionBuilder = new SagaVersionBuilder("1");
            sagaVersionBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Assert
            Assert.Equal("1", sagaVersionBuilder.Steps[0].Version);
        }
    }
}