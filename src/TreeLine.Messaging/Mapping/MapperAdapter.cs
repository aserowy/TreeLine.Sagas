using AutoMapper;
using System;

namespace TreeLine.Messaging.Mapping
{
    internal interface IMapperAdapter
    {
        object Map(object source, Type sourceType, Type destinationType);
    }

    internal sealed class MapperAdapter : IMapperAdapter
    {
        private readonly IMapperConfigurationProvider _configurationProvider;
        private readonly IMapper _mapper;

        public MapperAdapter(IMapperConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;

            _mapper = new Mapper(_configurationProvider.Get());
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, sourceType, destinationType);
        }
    }
}
