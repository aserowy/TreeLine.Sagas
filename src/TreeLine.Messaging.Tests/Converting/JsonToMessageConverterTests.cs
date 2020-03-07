using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using TreeLine.Messaging.Converting;
using TreeLine.Messaging.Mapping;
using TreeLine.Messaging.Tests.Mocks;
using Xunit;

namespace TreeLine.Messaging.Tests.Converting
{
    public class JsonToMessageConverterTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IConverter<string, JObject>> _mockConverter;
        private readonly Mock<IMessageTypeToClassResolver> _mockMessageTypeToClassResolver;
        private readonly Mock<IMapperAdapter> _mockMapperAdapter;

        public JsonToMessageConverterTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockConverter = _mockRepository.Create<IConverter<string, JObject>>();
            _mockMessageTypeToClassResolver = _mockRepository.Create<IMessageTypeToClassResolver>();
            _mockMapperAdapter = _mockRepository.Create<IMapperAdapter>();
        }

        private JsonToMessageConverter CreateJsonToMessageConverter()
        {
            return new JsonToMessageConverter(
                _mockConverter.Object,
                _mockMessageTypeToClassResolver.Object,
                _mockMapperAdapter.Object);
        }

        [Fact]
        public void Convert_ResolverReturnsMessage01Mock_MapResult()
        {
            // Arrange
            var jsonToMessageConverter = CreateJsonToMessageConverter();
            var input = $"{{'Type': {{'Type': 'TstType', 'Version': 'TstVersion', 'TargetType': {JsonConvert.SerializeObject(typeof(Message01Mock))}}}}}";

            var jObject = ResolveJObject(input);
            _mockConverter
                .Setup(cnvrtr => cnvrtr.Convert(input))
                .Returns(jObject);

            var destinationType = typeof(Message01Mock);
            _mockMessageTypeToClassResolver
                .Setup(rslvr => rslvr.Get("TstType", "TstVersion"))
                .Returns(destinationType);

            var mapped = new Message01Mock();
            _mockMapperAdapter
                .Setup(mppr => mppr.Map(jObject, jObject.GetType(), destinationType))
                .Returns(mapped);

            // Act
            var result = jsonToMessageConverter.Convert(input);

            // Assert
            Assert.Same(mapped, result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_ResolverReturnsNull_ThrowInvalidOperation()
        {
            // Arrange
            var jsonToMessageConverter = CreateJsonToMessageConverter();
            var input = $"{{'Type': {{'Type': 'TstType', 'Version': 'TstVersion', 'TargetType': {JsonConvert.SerializeObject(typeof(Message01Mock))}}}}}";

            _mockConverter
                .Setup(cnvrtr => cnvrtr.Convert(input))
                .Returns(ResolveJObject(input));

            _mockMessageTypeToClassResolver
                .Setup(rslvr => rslvr.Get("TstType", "TstVersion"))
                .Returns<Type?>(null);

            // Assert
            Assert.Throws<InvalidOperationException>(() => jsonToMessageConverter.Convert(input));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_JObjectWithTypePropertyNull_ThrowInvalidCast()
        {
            // Arrange
            var jsonToMessageConverter = CreateJsonToMessageConverter();
            var input = "{'Type': null}";

            _mockConverter
                .Setup(cnvrtr => cnvrtr.Convert(input))
                .Returns(ResolveJObject(input));

            // Assert
            Assert.Throws<InvalidCastException>(() => jsonToMessageConverter.Convert(input));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_JObjectWithTypePropertyButDifferentClass_ThrowInvalidCast()
        {
            // Arrange
            var jsonToMessageConverter = CreateJsonToMessageConverter();
            var input = "{'Type': 'Test'}";

            _mockConverter
                .Setup(cnvrtr => cnvrtr.Convert(input))
                .Returns(ResolveJObject(input));

            // Assert
            Assert.Throws<InvalidCastException>(() => jsonToMessageConverter.Convert(input));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Convert_JObjectWithoutTypeProperty_ThrowArgumentNull()
        {
            // Arrange
            var jsonToMessageConverter = CreateJsonToMessageConverter();
            var input = "{}";

            _mockConverter
                .Setup(cnvrtr => cnvrtr.Convert(input))
                .Returns(ResolveJObject(input));

            // Assert
            Assert.Throws<ArgumentNullException>(() => jsonToMessageConverter.Convert(input));

            _mockRepository.VerifyAll();
        }

        private static JObject ResolveJObject(string json)
        {
            using var stringReader = new StringReader(json);
            using var textReader = new JsonTextReader(stringReader);

            var serializer = new JsonSerializer { DateParseHandling = DateParseHandling.DateTimeOffset };
            var result = serializer.Deserialize<JObject>(textReader);
            if (result is null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            return result;
        }
    }
}
