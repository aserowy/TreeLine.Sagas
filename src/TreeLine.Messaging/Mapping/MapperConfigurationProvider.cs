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
        private readonly IMessageTypeResolver _resolver;

        private IConfigurationProvider? _mapperConfiguration;

        public MapperConfigurationProvider(IMessageTypeResolver resolver)
        {
            _resolver = resolver;
        }

        public IConfigurationProvider Get()
        {
            return _mapperConfiguration ?? (_mapperConfiguration = GenerateConfiguration());
        }

        private IConfigurationProvider GenerateConfiguration()
        {
            return new MapperConfiguration(cnfgrtn =>
            {
                // TODO: Resolve profiles dynamically if the list should be extended
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
