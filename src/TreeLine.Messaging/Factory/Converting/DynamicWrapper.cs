namespace TreeLine.Messaging.Factory.Converting
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