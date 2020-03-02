using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Messaging.Mapping;
using TreeLine.Messaging.Tests.Mocks;
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
            return new MapperConfigurationProvider(_mockMessageTypeResolver.Object);
        }

        [Fact]
        public void Get_CallingTwice_SameObjectReturned()
        {
            // Arrange
            var provider = CreateProvider();

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(Enumerable.Empty<IMessageType>().ToList());

            // Act
            var result01 = provider.Get();
            var result02 = provider.Get();

            // Assert
            Assert.Same(result01, result02);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_ProviderReturnsNothing_BaseProfilesMapped()
        {
            // Arrange
            var provider = CreateProvider();

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(Enumerable.Empty<IMessageType>().ToList());

            // Act
            var configuration = provider.Get();

            // Assert
            var mapping01 = configuration.FindTypeMapFor<JObject, MessageBase>();
            Assert.NotNull(mapping01);

            var mapping02 = configuration.FindTypeMapFor<JObject, MessageTypeBase>();
            Assert.NotNull(mapping02);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_ProviderReturnsType_TypeMappingAdded()
        {
            // Arrange
            var provider = CreateProvider();

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType> { new MessageType01Mock() });

            // Act
            var configuration = provider.Get();

            // Assert
            var mapping = configuration.FindTypeMapFor<JObject, Message01Mock>();
            Assert.NotNull(mapping);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_ProviderReturnsTypes_TypeMappingsAdded()
        {
            // Arrange
            var provider = CreateProvider();

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType>
                {
                    new MessageType01Mock(),
                    new MessageType02Mock()
                });

            // Act
            var configuration = provider.Get();

            // Assert
            var mapping01 = configuration.FindTypeMapFor<JObject, Message01Mock>();
            Assert.NotNull(mapping01);

            var mapping02 = configuration.FindTypeMapFor<JObject, Message02Mock>();
            Assert.NotNull(mapping02);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_ProviderReturnsTypeWithCustomClassProperty_PropertyIsMappedAsJToken()
        {
            // Arrange
            var provider = CreateProvider();

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType> { new MessageType02Mock() });

            // Act
            var configuration = provider.Get();

            // Assert
            var mapping = configuration.FindTypeMapFor<JToken, CustomClass>();
            Assert.NotNull(mapping);

            _mockRepository.VerifyAll();
        }
    }
}
