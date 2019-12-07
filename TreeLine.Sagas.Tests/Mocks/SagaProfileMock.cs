using TreeLine.Sagas.Builder;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaProfileMock : ISagaProfile
    {
        public void Configure(ISagaProcessorBuilder processorBuilder)
        {
            processorBuilder.AddStep<SagaEvent01, SagaStep01Mock>();
        }
    }
}