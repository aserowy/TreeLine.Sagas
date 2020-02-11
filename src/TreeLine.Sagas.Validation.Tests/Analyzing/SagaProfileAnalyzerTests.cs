using Moq;
using System;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Tests.Mock;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests.Analyzing
{
    public class SagaProfileAnalyzerTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaProfileVersionAnalyzerFactory> _mockSagaProfileVersionAnalyzerFactory;

        public SagaProfileAnalyzerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaProfileVersionAnalyzerFactory = _mockRepository.Create<ISagaProfileVersionAnalyzerFactory>();
        }

        private SagaProfileAnalyzer CreateSagaProfileAnalyzer(string name)
        {
            return new SagaProfileAnalyzer(name, _mockSagaProfileVersionAnalyzerFactory.Object);
        }

        [Fact]
        public void AddVersion_NoVersionAdded_VersionAnalyzerEmpty()
        {
            // Arrange
            var sagaProfileAnalyzer = CreateSagaProfileAnalyzer("");

            // Assert
            Assert.Equal(0, sagaProfileAnalyzer.VersionAnalyzer.Count);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddVersion_OneVersionAdded_VersionAdded()
        {
            // Arrange
            var sagaProfileAnalyzer = CreateSagaProfileAnalyzer("");

            _mockSagaProfileVersionAnalyzerFactory
                .Setup(mck => mck.Create(It.IsAny<string>()))
                .Returns((string vrsn) => new SagaProfileVersionAnalyzerMock(vrsn));

            // Act
            var result = sagaProfileAnalyzer.AddVersion("1") as SagaProfileVersionAnalyzerMock;

            // Assert
            Assert.Equal("1", result?.Version);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddVersion_TwoVersionsAdded_VersionsAdded()
        {
            // Arrange
            var sagaProfileAnalyzer = CreateSagaProfileAnalyzer("");

            _mockSagaProfileVersionAnalyzerFactory
                .Setup(mck => mck.Create(It.IsAny<string>()))
                .Returns((string vrsn) => new SagaProfileVersionAnalyzerMock(vrsn));

            _mockSagaProfileVersionAnalyzerFactory
                .Setup(mck => mck.Create(It.IsAny<string>()))
                .Returns((string vrsn) => new SagaProfileVersionAnalyzerMock(vrsn));

            // Act
            sagaProfileAnalyzer.AddVersion("1");
            sagaProfileAnalyzer.AddVersion("2");

            // Assert
            Assert.Equal(2, sagaProfileAnalyzer.VersionAnalyzer.Count);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ProfileName_InjectName_NameSet()
        {
            // Arrange
            var sagaProfileAnalyzer = CreateSagaProfileAnalyzer("Test");

            // Assert
            Assert.Equal("Test", sagaProfileAnalyzer.ProfileName);
        }

        [Fact]
        public void Build_Call_ThrowMethodAccessException()
        {
            // Arrange
            var sagaProfileAnalyzer = CreateSagaProfileAnalyzer("");

            // Assert
            Assert.Throws<MethodAccessException>(() => sagaProfileAnalyzer.Build());
        }
    }
}
