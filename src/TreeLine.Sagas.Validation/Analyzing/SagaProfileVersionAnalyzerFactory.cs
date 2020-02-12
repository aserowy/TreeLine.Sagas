namespace TreeLine.Sagas.Validation.Analyzing
{
    internal interface ISagaProfileVersionAnalyzerFactory
    {
        ISagaProfileVersionAnalyzer Create(string version);
    }

    internal sealed class SagaProfileVersionAnalyzerFactory : ISagaProfileVersionAnalyzerFactory
    {
        public ISagaProfileVersionAnalyzer Create(string version)
        {
            return new SagaProfileVersionAnalyzer(version);
        }
    }
}
