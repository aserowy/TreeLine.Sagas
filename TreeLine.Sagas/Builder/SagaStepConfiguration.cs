using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaStepConfiguration
    {
        Type SagaStepType { get; }

        bool IsResponsible(ISagaEvent sagaEvent);
        ISagaStepAdapter Create(ISagaServiceProvider provider);
    }

    internal sealed class SagaStepConfiguration<TEvent, TStep> : ISagaStepConfiguration
        where TEvent : ISagaEvent
        where TStep : ISagaStep<TEvent>
    {
        private readonly Predicate<TEvent>? _customValidation;

        public Type SagaStepType => typeof(TStep);

        public SagaStepConfiguration(Predicate<TEvent>? customValidation)
        {
            _customValidation = customValidation;
        }

        public bool IsResponsible(ISagaEvent sagaEvent)
        {
            if (!(sagaEvent is TEvent converted))
            {
                return false;
            }

            return _customValidation?.Invoke(converted) ?? true;
        }

        public ISagaStepAdapter Create(ISagaServiceProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return new SagaStepAdapter<TEvent>(provider.Resolve<TEvent, TStep>());
        }
    }
}