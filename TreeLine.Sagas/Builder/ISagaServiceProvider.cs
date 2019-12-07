using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaServiceProvider
    {
        ISagaProcessor ResolveProcessor();

        ISagaStep<TEvent> Resolve<TEvent, TSagaStep>()
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep<TEvent>;
    }
}