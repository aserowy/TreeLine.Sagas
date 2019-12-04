using System;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processor;

namespace TreeLine.Sagas
{
    public interface ISaga<TProfile>
    {
        Task RunAsync(ISagaEvent sagaEvent);
    }

    internal sealed class Saga<TProfile> : ISaga<TProfile> where TProfile : ISagaProfile
    {
        private readonly ISagaProfile _profile;
        private readonly ISagaProcessorBuilder _processorBuilder;

        private ISagaProcessor? _processor;

        public Saga(
            TProfile profile,
            ISagaProcessorBuilder processorBuilder)
        {
            _profile = profile;
            _processorBuilder = processorBuilder;
        }

        public Task RunAsync(ISagaEvent sagaEvent)
        {
            if (sagaEvent is null)
            {
                throw new ArgumentNullException(nameof(sagaEvent));
            }

            return GetOrCreateProcessor().RunAsync(sagaEvent);
        }

        private ISagaProcessor GetOrCreateProcessor()
        {
            if (_processor is { })
            {
                return _processor;
            }

            _profile.Configure(_processorBuilder);
            _processor = _processorBuilder.Build();

            return _processor;
        }
    }
}