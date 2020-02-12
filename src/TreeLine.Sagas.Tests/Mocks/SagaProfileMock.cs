using TreeLine.Sagas.Building;

namespace TreeLine.Sagas.Tests.Mocks
{
    internal sealed class SagaProfileMock : ISagaProfile
    {
        public void Configure(ISagaProcessorBuilder processorBuilder)
        {
            processorBuilder
                .AddVersion("1")
                .AddStep<SagaEvent01, SagaStep01Mock>();
        }
    }
}