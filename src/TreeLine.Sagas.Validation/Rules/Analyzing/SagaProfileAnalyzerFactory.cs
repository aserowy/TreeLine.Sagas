namespace TreeLine.Sagas.Validation.Rules.Analyzing
{
    internal interface ISagaProfileAnalyzerFactory
    {
        ISagaProfileAnalyzer Create();
    }

    internal sealed class SagaProfileAnalyzerFactory : ISagaProfileAnalyzerFactory
    {
        private readonly ISagaProfileVersionAnalyzerFactory _factory;

        public SagaProfileAnalyzerFactory(ISagaProfileVersionAnalyzerFactory factory)
        {
            _factory = factory;
        }

        public ISagaProfileAnalyzer Create()
        {
            return new SagaProfileAnalyzer(_factory);
        }
    }
}
