using System;
using System.Collections.Generic;

namespace TreeLine.Sagas
{
    public interface ISagaProcessorBuilder
    {
        ISagaProcessorBuilder AddStep<TEvent, TSagaStep>()
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep;

        ISagaProcessor Build();
    }

    internal sealed class SagaProcessorBuilder : ISagaProcessorBuilder
    {
        private readonly ISagaServiceProvider _serviceProvider;
        private readonly IList<Action<ISagaProcessor>> _actions;

        public SagaProcessorBuilder(ISagaServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _actions = new List<Action<ISagaProcessor>>();
        }

        public ISagaProcessorBuilder AddStep<TEvent, TSagaStep>()
            where TEvent : ISagaEvent
            where TSagaStep : ISagaStep
        {
            _actions.Add(cnfg => cnfg.Add(new[] { typeof(TEvent) }, _serviceProvider.Resolve<TSagaStep>()));

            return this;
        }

        public ISagaProcessor Build()
        {
            var processor = _serviceProvider.ResolveProcessor();
            foreach (var action in _actions)
            {
                action.Invoke(processor);
            }

            return processor;
        }
    }
}