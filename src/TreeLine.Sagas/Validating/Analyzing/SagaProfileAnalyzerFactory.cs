namespace TreeLine.Sagas.Validating.Analyzing
{
    internal interface ISagaProfileAnalyzerFactory
    {
        ISagaProfileAnalyzer Create(string profileName);
    }

    internal sealed class SagaProfileAnalyzerFactory : ISagaProfileAnalyzerFactory
    {
        private readonly ISagaProfileVersionAnalyzerFactory _factory;

        public SagaProfileAnalyzerFactory(ISagaProfileVersionAnalyzerFactory factory)
        {
            _factory = factory;
        }

        public ISagaProfileAnalyzer Create(string profileName)
        {
            return new SagaProfileAnalyzer(profileName, _factory);
        }
    }
}
