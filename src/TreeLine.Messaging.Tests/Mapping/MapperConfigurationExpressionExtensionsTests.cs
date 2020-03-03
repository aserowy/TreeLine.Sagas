using AutoMapper;
using Newtonsoft.Json.Linq;
using TreeLine.Messaging.Mapping;
using TreeLine.Messaging.Mapping.Profiles;
using TreeLine.Messaging.Tests.Mocks;
using Xunit;

namespace TreeLine.Messaging.Tests.Mapping
{
    public class MapperConfigurationExpressionExtensionsTests
    {
        [Fact]
        public void AddJObjectMapping_RegisterType01Mock_MappingForMessage01MockAdded()
        {
            // Arrange
            var type = new MessageType01Mock();

            // Act
            var result = new MapperConfiguration(cnfgrtn =>
            {
                cnfgrtn.AddProfile<JsonToMessageBaseProfile>();

                cnfgrtn.AddJObjectMapping(type);
            });

            // Assert
            var mapping = result.FindTypeMapFor<JObject, Message01Mock>();
            Assert.NotNull(mapping);
        }

        [Fact]
        public void AddJTokenMapping_RegisterCustomClass_MappingForCustomClassAdded()
        {
            // Arrange
            var type = typeof(CustomClass);

            // Act
            var result = new MapperConfiguration(cnfgrtn => cnfgrtn.AddJTokenMapping(new[] { type }));

            // Assert
            var mapping = result.FindTypeMapFor<JToken, CustomClass>();
            Assert.NotNull(mapping);
        }

        // AddJObjectMapping: Return of Custom class 
        // AddJObjectMapping: Exc when base profile is missing
        // AddJObjectMapping: Return empty if no custom class

        // AddJTokenMapping: Only system types, no mappings added
        // AddJTokenMapping: Multiple custom types
        // AddJTokenMapping: Same type multiple times, only one mapping is added
    }
}
