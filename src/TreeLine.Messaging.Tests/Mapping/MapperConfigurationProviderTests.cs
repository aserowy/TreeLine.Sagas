using Moq;
using TreeLine.Messaging.Mapping;
using Xunit;

namespace TreeLine.Messaging.Tests.Mapping
{
    public class MapperConfigurationProviderTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IMessageTypeResolver> _mockMessageTypeResolver;

        public MapperConfigurationProviderTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMessageTypeResolver = _mockRepository.Create<IMessageTypeResolver>();
        }

        private MapperConfigurationProvider CreateProvider()
        {
            return new MapperConfigurationProvider(
                _mockMessageTypeResolver.Object);
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var result = provider.Get();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
