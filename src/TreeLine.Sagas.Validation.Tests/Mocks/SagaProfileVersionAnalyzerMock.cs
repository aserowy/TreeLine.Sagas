using System;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Tests.Mock
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
            return this;
        }
    }
}
