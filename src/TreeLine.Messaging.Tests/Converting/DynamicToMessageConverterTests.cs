using Moq;
using System;
using System.Dynamic;
using TreeLine.Messaging.Converting;
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
            return new DynamicToMessageConverter(
                _mockMessageTypeToClassResolver.Object);
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
