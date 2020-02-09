using System;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;

namespace TreeLine.Sagas.Validation.Analyzing
{
    internal interface ISagaProfileVersionAnalyzer : ISagaVersionBuilder
    {
        int StepCount { get; }
        string Version { get; }
    }

    internal sealed class SagaProfileVersionAnalyzer : ISagaProfileVersionAnalyzer
    {
        public SagaProfileVersionAnalyzer(string version)
        {
            Version = version;
        }

        public int StepCount { get; private set; }
        public string Version { get; }

        public ISagaVersionBuilder AddStep<TEvent, TSagaStep>(Predicate<TEvent>? customValidation = null)
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep<TEvent>
        {
            StepCount++;

            return this;
        }
    }
}
