using TreeLine.Sagas.Validating.Analyzing;
using Xunit;

namespace TreeLine.Sagas.Tests.Validating.Analyzing
{
    public class SagaProfileVersionAnalyzerFactoryTests
    {
        private SagaProfileVersionAnalyzerFactory CreateFactory()
        {
            return new SagaProfileVersionAnalyzerFactory();
        }

        [Fact]
        public void Create_VersionIsGiven_VersionIsMapped()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result = factory.Create("Test");

            // Assert
            Assert.Equal("Test", result.Version);
        }

        [Fact]
        public void Create_TwoTimesCalled_ResultsAreNotSame()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result01 = factory.Create("Test");
            var result02 = factory.Create("Test");

            // Assert
            Assert.NotSame(result01, result02);
        }
    }
}
