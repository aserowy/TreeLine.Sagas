using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void AddJObjectMapping_MessageWithoutCustomTypeProperty_ReturnEmpty()
        {
            // Arrange
            var type = new MessageType01Mock();

            // Act
            IEnumerable<Type>? result = null;
            _ = new MapperConfiguration(cnfgrtn =>
            {
                cnfgrtn.AddProfile<JsonToMessageBaseProfile>();

                result = cnfgrtn.AddJObjectMapping(type);
            });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AddJObjectMapping_MessageWithCustomTypeProperty_ReturnCustomType()
        {
            // Arrange
            var type = new MessageType02Mock();

            // Act
            IEnumerable<Type>? result = null;
            _ = new MapperConfiguration(cnfgrtn =>
            {
                cnfgrtn.AddProfile<JsonToMessageBaseProfile>();

                result = cnfgrtn.AddJObjectMapping(type);
            });

            // Assert
            Assert.Single(result);
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

        [Fact]
        public void AddJTokenMapping_SameCustomClassRegisteredTwice_OnlyOneMappingAdded()
        {
            // Arrange
            var type = typeof(CustomClass);

            // Act
            var result = new MapperConfiguration(cnfgrtn => cnfgrtn.AddJTokenMapping(new[] { type, type }));

            // Assert
            var mappings = result
                .GetAllTypeMaps()
                .Where(typMp => typMp.DestinationType.Equals(type));

            Assert.Single(mappings);
        }

        [Fact]
        public void AddJTokenMapping_RegisterSystemType_NoMappingsAdded()
        {
            // Arrange
            var type = typeof(DateTime);

            // Act
            var result = new MapperConfiguration(cnfgrtn => cnfgrtn.AddJTokenMapping(new[] { type }));

            // Assert
            var mapping = result.FindTypeMapFor<JToken, int>();
            Assert.Null(mapping);
        }

        [Fact]
        public void AddJTokenMapping_DifferentCustomTypesRegistered_MultipleMappingsAdded()
        {
            // Arrange
            var type01 = typeof(CustomClass);
            var type02 = typeof(Message01Mock);

            // Act
            var result = new MapperConfiguration(cnfgrtn => cnfgrtn.AddJTokenMapping(new[] { type01, type02 }));

            // Assert
            var mappings = result
                .GetAllTypeMaps()
                .Where(typMp => typMp.DestinationType.Equals(type01) || typMp.DestinationType.Equals(type02));

            Assert.Equal(2, mappings.Count());
        }
    }
}
