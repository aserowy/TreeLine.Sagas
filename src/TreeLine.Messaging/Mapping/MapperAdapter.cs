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
        private readonly IMapper _mapper;

        public MapperAdapter(IMapperConfigurationProvider configurationProvider)
        {
            _mapper = new Mapper(configurationProvider.Get());
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, sourceType, destinationType);
        }
    }
}
