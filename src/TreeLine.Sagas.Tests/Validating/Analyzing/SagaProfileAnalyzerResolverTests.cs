using Moq;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Validating.Analyzing;
using Xunit;

namespace TreeLine.Sagas.Tests.Validating.Analyzing
{
    public class SagaProfileAnalyzerResolverTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaProfileAnalyzerFactory> _mockSagaProfileAnalyzerFactory;
        private readonly Mock<IEnumerable<ISagaProfile>> _mockEnumerable;

        public SagaProfileAnalyzerResolverTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaProfileAnalyzerFactory = _mockRepository.Create<ISagaProfileAnalyzerFactory>();
            _mockEnumerable = _mockRepository.Create<IEnumerable<ISagaProfile>>();
        }

        private SagaProfileAnalyzerResolver CreateSagaProfileAnalyzerResolver()
        {
            return new SagaProfileAnalyzerResolver(
                _mockSagaProfileAnalyzerFactory.Object,
                _mockEnumerable.Object);
        }

        private IEnumerable<Mock<ISagaProfile>> GetProfileMocks(int count)
        {
            var result = new List<Mock<ISagaProfile>>();
            for (int i = 0; i < count; i++)
            {
                var mock = _mockRepository.Create<ISagaProfile>();
                mock.Setup(mck => mck.Configure(It.IsAny<ISagaProfileAnalyzer>()));

                result.Add(mock);
            }

            return result;
        }

        [Fact]
        public void Get_NoProfiles_NoAnalyzersReturned()
        {
            // Arrange
            var sagaProfileAnalyzerResolver = CreateSagaProfileAnalyzerResolver();

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(new List<ISagaProfile>().GetEnumerator());

            // Act
            var result = sagaProfileAnalyzerResolver.Get();

            // Assert
            Assert.Empty(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_OneProfile_OneAnalyzerReturned()
        {
            // Arrange
            var sagaProfileAnalyzerResolver = CreateSagaProfileAnalyzerResolver();

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(GetProfileMocks(1).Select(mck => mck.Object).GetEnumerator());

            _mockSagaProfileAnalyzerFactory
                .Setup(mck => mck.Create(It.IsAny<string>()))
                .Returns((string prfl) => new SagaProfileAnalyzerMock(prfl));

            // Act
            var result = sagaProfileAnalyzerResolver.Get();

            // Assert
            Assert.Single(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_TwoProfiles_OneAnalyzerReturned()
        {
            // Arrange
            var sagaProfileAnalyzerResolver = CreateSagaProfileAnalyzerResolver();

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(GetProfileMocks(2).Select(mck => mck.Object).GetEnumerator());

            _mockSagaProfileAnalyzerFactory
                .Setup(mck => mck.Create(It.IsAny<string>()))
                .Returns((string prfl) => new SagaProfileAnalyzerMock(prfl));

            _mockSagaProfileAnalyzerFactory
                .Setup(mck => mck.Create(It.IsAny<string>()))
                .Returns((string prfl) => new SagaProfileAnalyzerMock(prfl));

            // Act
            var result = sagaProfileAnalyzerResolver.Get();

            // Assert
            Assert.Equal(2, result.Count());

            _mockRepository.VerifyAll();
        }
    }
}
