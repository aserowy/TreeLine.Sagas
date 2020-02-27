using AutoMapper;
using TreeLine.Messaging.Mapping.Profiles;

namespace TreeLine.Messaging.Mapping
{
    internal interface IMapperConfigurationProvider
    {
        IConfigurationProvider Get();
    }

    internal sealed class MapperConfigurationProvider : IMapperConfigurationProvider
    {
        private readonly object _lock = new object();
        private readonly IMessageTypeResolver _resolver;

        private IConfigurationProvider? _mapperConfiguration;

        public MapperConfigurationProvider(IMessageTypeResolver resolver)
        {
            _resolver = resolver;
        }

        public IConfigurationProvider Get()
        {
            if (_mapperConfiguration is null)
            {
                lock (_lock)
                {
                    if (_mapperConfiguration is null)
                    {
                        _mapperConfiguration = GenerateConfiguration();
                    }
                }
            }

            return _mapperConfiguration;
        }

        private IConfigurationProvider GenerateConfiguration()
        {
            return new MapperConfiguration(cnfgrtn =>
            {
                // If the list must be extended, resolve profiles dynamically instead
                cnfgrtn.AddProfile<JsonToMessageBaseProfile>();
                cnfgrtn.AddProfile<JsonToMessageTypeProfile>();

                foreach (var type in _resolver.Get())
                {
                    cnfgrtn.AddMessageTypeMappings(type);
                }
            });
        }
    }
}
