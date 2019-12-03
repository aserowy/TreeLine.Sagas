using TreeLine.Sagas.Processor;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaServiceProvider
    {
        ISagaProcessor ResolveProcessor();
        ISagaStep Resolve<TSagaStep>() where TSagaStep : ISagaStep;
    }
}