namespace TreeLine.Messaging.Converting
{
    internal interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn input);
    }
}