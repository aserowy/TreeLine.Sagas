namespace TreeLine.Sagas.Versioning
{
    internal interface ISagaVersionFactory
    {
        ISagaVersion Create(string version);
    }

    internal sealed class SagaVersionFactory : ISagaVersionFactory
    {
        public ISagaVersion Create(string version) => new SagaVersion(version);
    }
}