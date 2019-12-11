using System;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaStepConfiguration
    {
        ISagaVersion Version { get; }
        Type SagaStepType { get; }

        bool IsResponsible(ISagaEvent sagaEvent);
        ISagaStepAdapter Create(ISagaServiceProvider provider);
    }

    internal sealed class SagaStepConfiguration<TEvent, TStep> : ISagaStepConfiguration
        where TEvent : ISagaEvent
        where TStep : ISagaStep<TEvent>
    {
        private readonly Predicate<TEvent>? _customValidation;

        public SagaStepConfiguration(ISagaVersion version, int index, Predicate<TEvent>? customValidation)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Index = index;

            _customValidation = customValidation;
        }

        public ISagaVersion Version { get; }
        public Type SagaStepType => typeof(TStep);
        public int Index { get; }

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

            return new SagaStepAdapter<TEvent>(Version, Index, provider.Resolve<TEvent, TStep>());
        }
    }
}