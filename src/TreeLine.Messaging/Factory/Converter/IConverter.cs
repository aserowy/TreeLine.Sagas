namespace TreeLine.Messaging.Factory.Converter
{
    internal interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn input);
    }
}