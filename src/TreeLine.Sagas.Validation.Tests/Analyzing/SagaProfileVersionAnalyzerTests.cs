using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests.Analyzing
{
    public class SagaProfileVersionAnalyzerTests
    {
        private SagaProfileVersionAnalyzer CreateSagaProfileVersionAnalyzer()
        {
            return new SagaProfileVersionAnalyzer("Test");
        }

        [Fact]
        public void Ctor_VersionAdded_VersionIsMapped()
        {
            // Arrange
            var sagaProfileVersionAnalyzer = new SagaProfileVersionAnalyzer("Test");

            // Assert
            Assert.Equal("Test", sagaProfileVersionAnalyzer.Version);
        }

        [Fact]
        public void AddStep_NoStepAdded_StepCountIsZero()
        {
            // Arrange
            var sagaProfileVersionAnalyzer = CreateSagaProfileVersionAnalyzer();

            // Assert
            Assert.Equal(0, sagaProfileVersionAnalyzer?.StepCount);
        }

        [Fact]
        public void AddStep_OneStepAdded_StepCountIsOne()
        {
            // Arrange
            var sagaProfileVersionAnalyzer = CreateSagaProfileVersionAnalyzer();

            // Act
            var result = sagaProfileVersionAnalyzer.AddStep<SagaEvent01, SagaStep01Mock>() as SagaProfileVersionAnalyzer;

            // Assert
            Assert.Equal(1, result?.StepCount);
        }

        [Fact]
        public void AddStep_TwoStepsAdded_StepCountIsTwo()
        {
            // Arrange
            var sagaProfileVersionAnalyzer = CreateSagaProfileVersionAnalyzer();

            // Act
            var result = sagaProfileVersionAnalyzer
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent01, SagaStep01Mock>()
                as SagaProfileVersionAnalyzer;

            // Assert
            Assert.Equal(2, result?.StepCount);
        }
    }
}
