using Moq;
using System;
using System.Collections.Generic;
using TreeLine.Messaging.Converting;
using TreeLine.Messaging.Tests.Mocks;
using Xunit;

namespace TreeLine.Messaging.Tests.Converting
{
    public class MessageTypeToClassResolverTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IMessageTypeResolver> _mockMessageTypeResolver;

        public MessageTypeToClassResolverTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMessageTypeResolver = _mockRepository.Create<IMessageTypeResolver>();
        }

        private MessageTypeToClassResolver CreateMessageTypeToClassResolver()
        {
            return new MessageTypeToClassResolver(
                _mockMessageTypeResolver.Object);
        }

        [Fact]
        public void Get_ResolvesMoreThanOneType_ThrowsInvalidOperation()
        {
            // Arrange
            var messageTypeToClassResolver = CreateMessageTypeToClassResolver();
            var type = "MessageMock";
            var version = "1";

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType> { new MessageType01Mock(), new MessageType01Mock() });

            // Assert
            Assert.Throws<InvalidOperationException>(() => messageTypeToClassResolver.Get(type, version));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_MessageTypeWithEqualTypeAndVersion_ReturnType()
        {
            // Arrange
            var messageTypeToClassResolver = CreateMessageTypeToClassResolver();
            var type = "Message02Mock";
            var version = "1";

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType> { new MessageType01Mock(), new MessageType02Mock() });

            // Act
            var result = messageTypeToClassResolver.Get(type, version);

            // Assert
            Assert.Equal(typeof(Message02Mock), result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_NoMessageTypeWithEqualTypeAndVersion_ReturnNull()
        {
            // Arrange
            var messageTypeToClassResolver = CreateMessageTypeToClassResolver();
            var type = "Type";
            var version = "Version";

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType> { new MessageType01Mock() });

            // Act
            var result = messageTypeToClassResolver.Get(type, version);

            // Assert
            Assert.Null(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Get_ResolverReturnsEmpty_ReturnNull()
        {
            // Arrange
            var messageTypeToClassResolver = CreateMessageTypeToClassResolver();
            var type = "Type";
            var version = "Version";

            _mockMessageTypeResolver
                .Setup(rslvr => rslvr.Get())
                .Returns(new List<IMessageType>());

            // Act
            var result = messageTypeToClassResolver.Get(type, version);

            // Assert
            Assert.Null(result);

            _mockRepository.VerifyAll();
        }
    }
}
