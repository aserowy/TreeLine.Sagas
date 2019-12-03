using System.Threading.Tasks;

namespace TreeLine.Sagas
{
    public interface ISaga<TProfile>
    {
        Task RunAsync(ISagaEvent sagaEvent);
    }

    internal sealed class Saga<TProfile> : ISaga<TProfile>
    {
        private readonly ISagaProfile _profile;
        private readonly ISagaProcessorBuilder _processorBuilder;

        private ISagaProcessor? _processor;

        public Saga(
            ISagaProfile profile,
            ISagaProcessorBuilder processorBuilder)
        {
            _profile = profile;
            _processorBuilder = processorBuilder;
        }

        public Task RunAsync(ISagaEvent sagaEvent)
        {
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