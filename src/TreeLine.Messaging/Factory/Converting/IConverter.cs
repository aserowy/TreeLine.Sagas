namespace TreeLine.Messaging.Factory.Converting
{
    internal interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn input);
    }
}