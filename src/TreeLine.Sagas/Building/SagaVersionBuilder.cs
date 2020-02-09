using System;
using System.Collections.Generic;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Building
{
    public interface ISagaVersionBuilder
    {
        ISagaVersionBuilder AddStep<TEvent, TSagaStep>(Predicate<TEvent>? customValidation = null)
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep<TEvent>;
    }

    internal sealed class SagaVersionBuilder : ISagaVersionBuilder
    {
        private int _index;

        public SagaVersionBuilder(ISagaVersion version)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));

            Steps = new List<ISagaStepConfiguration>();
        }

        public ISagaVersion Version { get; }

        public IList<ISagaStepConfiguration> Steps { get; }

        public ISagaVersionBuilder AddStep<TEvent, TSagaStep>(Predicate<TEvent>? customValidation = null)
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep<TEvent>
        {
            Steps.Add(new SagaStepConfiguration<TEvent, TSagaStep>(Version, _index++, customValidation));

            return this;
        }
    }
}