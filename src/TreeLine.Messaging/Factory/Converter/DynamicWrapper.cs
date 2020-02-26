namespace TreeLine.Messaging.Factory.Converter
{
    public class DynamicWrapper
    {
        public DynamicWrapper(dynamic data)
        {
            Data = data;
        }

        public dynamic Data { get; }
    }
}