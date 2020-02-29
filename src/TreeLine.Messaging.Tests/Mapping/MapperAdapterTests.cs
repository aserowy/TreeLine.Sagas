using AutoMapper;
using Moq;
using TreeLine.Messaging.Mapping;
using Xunit;

namespace TreeLine.Messaging.Tests.Mapping
{
    public class MapperAdapterTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IMapperConfigurationProvider> _mockMapperConfigurationProvider;

        public MapperAdapterTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMapperConfigurationProvider = _mockRepository.Create<IMapperConfigurationProvider>();
        }

        private MapperAdapter CreateMapperAdapter()
        {
            return new MapperAdapter(
                _mockMapperConfigurationProvider.Object);
        }

        [Fact]
        public void Map_MapperIsCalled_ReturnsInt()
        {
            // Arrange
            _mockMapperConfigurationProvider
                .Setup(prvdr => prvdr.Get())
                .Returns(new MapperConfiguration(exprssn => { }));

            var mapperAdapter = CreateMapperAdapter();
            var source = 1;
            var sourceType = typeof(object);
            var destinationType = typeof(int);

            // Act
            var result = mapperAdapter.Map(
                source,
                sourceType,
                destinationType);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Map_CalledTwice_ProviderCalledOnce()
        {
            // Arrange
            _mockMapperConfigurationProvider
                .Setup(prvdr => prvdr.Get())
                .Returns(new MapperConfiguration(exprssn => { }));

            var mapperAdapter = CreateMapperAdapter();
            var sourceType = typeof(object);
            var destinationType = typeof(int);

            // Act
            _ = mapperAdapter.Map(
                1,
                sourceType,
                destinationType);

            _ = mapperAdapter.Map(
                2,
                sourceType,
                destinationType);

            // Assert
            _mockMapperConfigurationProvider.Verify(prvdr => prvdr.Get(), Times.Once);
            _mockRepository.VerifyAll();
        }
    }
}
