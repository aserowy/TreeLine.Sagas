using System;
using System.Collections.Generic;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;

namespace TreeLine.Sagas.Builder
{
    public interface ISagaProcessorBuilder
    {
        ISagaProcessorBuilder AddStep<TEvent, TSagaStep>(Predicate<TEvent>? customValidation = null)
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep;

        ISagaProcessor Build();
    }

    internal sealed class SagaProcessorBuilder : ISagaProcessorBuilder
    {
        private readonly ISagaServiceProvider _serviceProvider;
        private readonly IList<ISagaStepConfiguration> _steps;

        public SagaProcessorBuilder(ISagaServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _steps = new List<ISagaStepConfiguration>();
        }

        public ISagaProcessorBuilder AddStep<TEvent, TSagaStep>(Predicate<TEvent>? customValidation = null)
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep
        {
            _steps.Add(new SagaStepConfiguration<TEvent, TSagaStep>(customValidation));

            return this;
        }

        public ISagaProcessor Build()
        {
            var processor = _serviceProvider.ResolveProcessor();
            processor.AddSteps(_steps);

            return processor;
        }
    }
}