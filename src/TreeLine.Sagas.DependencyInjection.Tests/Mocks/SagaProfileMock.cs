using TreeLine.Sagas.Building;

namespace TreeLine.Sagas.DependencyInjection.Tests.Mocks
{
    internal sealed class SagaProfileMock : ISagaProfile
    {
        public void Configure(ISagaProcessorBuilder processorBuilder)
        {
            processorBuilder
                .AddVersion("1.0.0")
                .AddStep<SagaEvent01, SagaStep01Mock>();
        }
    }
}