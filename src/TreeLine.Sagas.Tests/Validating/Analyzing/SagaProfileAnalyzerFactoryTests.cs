using Moq;
using TreeLine.Sagas.Validating.Analyzing;
using Xunit;

namespace TreeLine.Sagas.Tests.Validating.Analyzing
{
    public class SagaProfileAnalyzerFactoryTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaProfileVersionAnalyzerFactory> _mockSagaProfileVersionAnalyzerFactory;

        public SagaProfileAnalyzerFactoryTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaProfileVersionAnalyzerFactory = _mockRepository.Create<ISagaProfileVersionAnalyzerFactory>();
        }

        private SagaProfileAnalyzerFactory CreateFactory()
        {
            return new SagaProfileAnalyzerFactory(_mockSagaProfileVersionAnalyzerFactory.Object);
        }

        [Fact]
        public void Create_Called_VersionAnalyzerFactoryNotCalled()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            factory.Create("Test");

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_CalledTwice_ReturnsNotSameObjects()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result01 = factory.Create("Test");
            var result02 = factory.Create("Test");

            // Assert
            Assert.NotSame(result01, result02);

            _mockRepository.VerifyAll();
        }
    }
}
