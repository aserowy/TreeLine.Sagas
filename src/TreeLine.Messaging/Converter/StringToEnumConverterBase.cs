using System;
using AutoMapper;

namespace TreeLine.Messaging.Converter
{
    public class StringToEnumConverterBase<TEnum> : IValueConverter<string, TEnum>
    {
        public TEnum Convert(string sourceMember, ResolutionContext context)
        {
            return Enum.TryParse(typeof(TEnum), sourceMember, out var result) ? (TEnum) result : default;
        }
    }
}