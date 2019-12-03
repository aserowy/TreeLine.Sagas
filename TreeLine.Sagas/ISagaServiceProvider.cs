namespace TreeLine.Sagas
{
    public interface ISagaServiceProvider
    {
        ISagaProcessor ResolveProcessor();
        ISagaStep Resolve<TSagaStep>() where TSagaStep : ISagaStep;
    }
}