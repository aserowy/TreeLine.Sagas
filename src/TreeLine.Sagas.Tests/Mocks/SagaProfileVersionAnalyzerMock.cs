using System;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Validating.Analyzing;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaProfileVersionAnalyzerMock : ISagaProfileVersionAnalyzer
    {
        public SagaProfileVersionAnalyzerMock(string version)
        {
            Version = version;
        }

        public int StepCount { get; set; }
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
