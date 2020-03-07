using Moq;
using System;
using System.Dynamic;
using TreeLine.Messaging.Converting;
using TreeLine.Messaging.Tests.Mocks;
using Xunit;

namespace TreeLine.Messaging.Tests.Converting
{
    public class DynamicToMessageConverterTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IMessageTypeToClassResolver> _mockMessageTypeToClassResolver;

        public DynamicToMessageConverterTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMessageTypeToClassResolver = _mockRepository.Create<IMessageTypeToClassResolver>();
        }

        private DynamicToMessageConverter CreateDynamicToMessageConverter()
        {
            return new DynamicToMessageConverter(_mockMessageTypeToClassResolver.Object);
        }

        [Fact]
        public void Convert_ValidMapping_PropertiesAreMapped()
        {
            // Arrange
            _mockMessageTypeToClassResolver
                .Setup(rslvr => rslvr.Get("TestType", "TestVersion"))
                .Returns(typeof(Message01Mock));

            var dynamicToMessageConverter = CreateDynamicToMessageConverter();

            var id = Guid.NewGuid();
            var time = DateTimeOffset.Now;
            var wrapper = new DynamicWrapper(
                new
                {
                    Type = new
                    {
                        Type = "TestType",
                        Version = "TestVersion",
                        TargetType = typeof(Message01Mock)
                    },
                    Id = id,
                    TimeOffset = time
                });

            // Act
            var result = dynamicToMessageConverter.Convert(wrapper) as Message01Mock;

            // Assert
            Assert.Equal(id, result?.Id);
            Assert.Equal(time, result?.TimeOffset);
            Assert.Equal("TestType", result?.Type.Type);
            Assert.Equal("TestVersion", result?.Type.Version);
            Assert.Equal(typeof(Message01Mock), result?.Type.TargetType);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_TargetTypeForTypeAndVersion_ReturnMappedResult()
        {
            // Arrange
            _mockMessageTypeToClassResolver
                .Setup(rslvr => rslvr.Get("TestType", "TestVersion"))
                .Returns(typeof(Message01Mock));

            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            var id = Guid.NewGuid();
            var wrapper = new DynamicWrapper(
                new
                {
                    Type = new
                    {
                        Type = "TestType",
                        Version = "TestVersion",
                        TargetType = typeof(Message01Mock)
                    },
                    Id = id,
                    TimeOffset = DateTimeOffset.Now
                });

            // Act
            var result = dynamicToMessageConverter.Convert(wrapper);

            // Assert
            Assert.IsType<Message01Mock>(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_NoTargetTypeForTypeAndVersion_ThrowInvalidOperation()
        {
            // Arrange
            _mockMessageTypeToClassResolver
                .Setup(rslvr => rslvr.Get("TestType", "TestVersion"))
                .Returns<Type?>(null);

            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            var wrapper = new DynamicWrapper(
                new
                {
                    Type = new
                    {
                        Type = "TestType",
                        Version = "TestVersion"
                    }
                });

            // Assert
            Assert.Throws<InvalidOperationException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_DataAsObjectTypeIsNull_ThrowArgumentNull()
        {
            // Arrange
            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            var wrapper = new DynamicWrapper(new { });

            // Assert
            Assert.Throws<ArgumentNullException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_DataAsObjectTypeVersionIsNull_ThrowArgumentNull()
        {
            // Arrange
            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            var wrapper = new DynamicWrapper(new { Type = new { Type = "TestType" } });

            // Assert
            Assert.Throws<ArgumentNullException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_DataAsObjectTypeTypeIsNull_ThrowArgumentNull()
        {
            // Arrange
            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            var wrapper = new DynamicWrapper(new { Type = new { Version = "TestVersion" } });

            // Assert
            Assert.Throws<ArgumentNullException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_DataAsDynamicTypeIsNull_ThrowArgumentNull()
        {
            // Arrange
            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            var obj = new ExpandoObject();
            var wrapper = new DynamicWrapper(obj);

            // Assert
            Assert.Throws<ArgumentNullException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_DataAsDynamicTypeVersionIsNull_ThrowArgumentNull()
        {
            // Arrange
            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            dynamic type = new ExpandoObject();
            type.Type = "TestType";

            dynamic obj = new ExpandoObject();
            obj.Type = type;

            var wrapper = new DynamicWrapper(obj);

            // Assert
            Assert.Throws<ArgumentNullException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_DataAsDynamicTypeTypeIsNull_ThrowArgumentNull()
        {
            // Arrange
            var dynamicToMessageConverter = CreateDynamicToMessageConverter();
            dynamic type = new ExpandoObject();
            type.Version = "TestVersion";

            dynamic obj = new ExpandoObject();
            obj.Type = type;

            var wrapper = new DynamicWrapper(obj);

            // Assert
            Assert.Throws<ArgumentNullException>(() => dynamicToMessageConverter.Convert(wrapper));

            _mockRepository.VerifyAll();
        }
    }
}
