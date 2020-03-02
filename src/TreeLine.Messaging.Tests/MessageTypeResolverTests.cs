using Xunit;

namespace TreeLine.Messaging.Tests
{
    public class MessageTypeResolverTests
    {
        private MessageTypeResolver CreateMessageTypeResolver()
        {
            return new MessageTypeResolver(GetType().Assembly, typeof(IMessageType).Assembly);
        }

        [Fact]
        public void Get_AbstractAndNonAbstractTypesDefined_ResolveNonAbstractTypes()
        {
            // Arrange
            var messageTypeResolver = CreateMessageTypeResolver();

            // Act
            var result = messageTypeResolver.Get();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Get_CalledTwice_ResolveSameTypeObject()
        {
            // Arrange
            var messageTypeResolver = CreateMessageTypeResolver();

            // Act
            var call01 = messageTypeResolver.Get();
            var call02 = messageTypeResolver.Get();

            // Assert
            Assert.Same(call01, call02);
            Assert.Same(call01[0], call02[0]);
        }
    }
}
