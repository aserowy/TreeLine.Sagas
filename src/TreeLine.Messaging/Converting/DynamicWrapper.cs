namespace TreeLine.Messaging.Converting
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