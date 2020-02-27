using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace TreeLine.Messaging.Converting
{
    internal class StringToJObjectConverter : IConverter<string, JObject>
    {
        private JsonSerializer? _serializer;

        private JsonSerializer Serializer => _serializer ?? (_serializer = new JsonSerializer { DateParseHandling = DateParseHandling.DateTimeOffset });

        public JObject Convert(string input)
        {
            using var stringReader = new StringReader(input);
            using var textReader = new JsonTextReader(stringReader);

            var result = Serializer.Deserialize<JObject>(textReader);
            if (result is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return result;
        }
    }
}