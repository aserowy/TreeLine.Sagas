using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaServiceProvider
    {
        ISagaStep<TEvent> Resolve<TEvent, TSagaStep>()
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep<TEvent>;
    }
}