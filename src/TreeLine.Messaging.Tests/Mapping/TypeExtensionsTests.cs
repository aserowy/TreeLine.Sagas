using SystemTest;
using TreeLine.Messaging.Mapping;
using Xunit;

namespace TreeLine.Messaging.Tests.Mapping
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsCustom_TypeOfSystem_False()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = type.IsCustom();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsCustom_TypeOfSystemData_False()
        {
            // Arrange
            var type = typeof(System.Data.Constraint);

            // Act
            var result = type.IsCustom();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsCustom_NamespaceDoesNotStartWithSystem_True()
        {
            // Arrange
            var type = typeof(TypeExtensionsTests);

            // Act
            var result = type.IsCustom();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsCustom_NamespaceStartsWithSystemTest_True()
        {
            // Arrange
            var type = typeof(TypeWithNamespaceSystemTest);

            // Act
            var result = type.IsCustom();

            // Assert
            Assert.True(result);
        }
    }
}
