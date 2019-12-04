using System;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaStepConfiguration
    {
        ISagaStep Create(ISagaServiceProvider provider);
        bool IsResponsible(ISagaEvent sagaEvent);
    }

    internal sealed class SagaStepConfiguration<TEvent, TStep> : ISagaStepConfiguration
        where TEvent : ISagaEvent
        where TStep : ISagaStep
    {
        private readonly Predicate<TEvent>? _customValidation;

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

        public ISagaStep Create(ISagaServiceProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return provider.Resolve<TStep>();
        }
    }
}