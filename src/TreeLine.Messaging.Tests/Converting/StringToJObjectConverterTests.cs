using System;
using TreeLine.Messaging.Converting;
using Xunit;

namespace TreeLine.Messaging.Tests.Converting
{
    public class StringToJObjectConverterTests
    {
        private StringToJObjectConverter CreateStringToJObjectConverter()
        {
            return new StringToJObjectConverter();
        }

        [Fact]
        public void Convert_JsonWithMultipleProperties_PropertiesMapped()
        {
            // Arrange
            var stringToJObjectConverter = CreateStringToJObjectConverter();
            var input = "{'Name': 'Test', 'Foo': 'bar'}";

            // Act
            var result = stringToJObjectConverter.Convert(input);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Convert_JsonWithProperty_PropertyMapped()
        {
            // Arrange
            var stringToJObjectConverter = CreateStringToJObjectConverter();
            var input = "{'Name': 'Test'}";

            // Act
            var jObject = stringToJObjectConverter.Convert(input);
            var result = jObject.GetValue("Name", StringComparison.InvariantCulture);

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void Convert_EmptyJson_ReturnsEmptyJObject()
        {
            // Arrange
            var stringToJObjectConverter = CreateStringToJObjectConverter();
            var input = "{}";

            // Act
            var result = stringToJObjectConverter.Convert(input);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Convert_EmptyString_ThrowArgumentNull()
        {
            // Arrange
            var stringToJObjectConverter = CreateStringToJObjectConverter();
            var input = "";

            // Assert
            Assert.Throws<ArgumentNullException>(() => stringToJObjectConverter.Convert(input));
        }
    }
}
