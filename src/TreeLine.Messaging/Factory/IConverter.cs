namespace TreeLine.Messaging.Factory
{
    internal interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn input);
    }
}