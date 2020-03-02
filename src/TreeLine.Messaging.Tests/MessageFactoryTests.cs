using Moq;
using System;
using TreeLine.Messaging.Converting;
using TreeLine.Messaging.Tests.Mocks;
using Xunit;

namespace TreeLine.Messaging.Tests
{
    public class MessageFactoryTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IConverter<string, IMessage>> _mockConverterStringMessage;
        private readonly Mock<IConverter<DynamicWrapper, IMessage>> _mockConverterDynamicWrapperMessage;

        public MessageFactoryTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockConverterStringMessage = _mockRepository.Create<IConverter<string, IMessage>>();
            _mockConverterDynamicWrapperMessage = _mockRepository.Create<IConverter<DynamicWrapper, IMessage>>();
        }

        private MessageFactory CreateFactory()
        {
            return new MessageFactory(
                _mockConverterStringMessage.Object,
                _mockConverterDynamicWrapperMessage.Object);
        }

        [Fact]
        public void CreateDynamic_ContentIsNull_ThrowArgumentNull()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

            // Arrange
            var factory = CreateFactory();
            dynamic content = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create(content));

            _mockRepository.VerifyAll();

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        [Fact]
        public void CreateDynamic_ContentIsValid_ConverterIsCalled()
        {
            // Arrange
            var factory = CreateFactory();
            dynamic content = new object();

            var resultMock = new Message01Mock();

            _mockConverterDynamicWrapperMessage
                .Setup(cnvrtr => cnvrtr.Convert(It.IsAny<DynamicWrapper>()))
                .Returns(resultMock);

            // Act
            var result = factory.Create(content);

            // Assert
            Assert.Same(resultMock, result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void CreateJson_JsonIsNull_ThrowArgumentNull()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

            // Arrange
            var factory = CreateFactory();
            string json = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create(json));

            _mockRepository.VerifyAll();

#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        [Fact]
        public void CreateJson_JsonIsEmptyString_ThrowArgumentNull()
        {
            // Arrange
            var factory = CreateFactory();
            var json = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create(json));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void CreateJson_JsonIsValid_ConverterIsCalled()
        {
            // Arrange
            var factory = CreateFactory();
            var json = "{ }";

            var resultMock = new Message01Mock();

            _mockConverterStringMessage
                .Setup(cnvrtr => cnvrtr.Convert(It.Is<string>(strng => strng.Equals(json, StringComparison.InvariantCulture))))
                .Returns(resultMock);

            // Act
            var result = factory.Create(json);

            // Assert
            Assert.Same(resultMock, result);

            _mockRepository.VerifyAll();
        }
    }
}
