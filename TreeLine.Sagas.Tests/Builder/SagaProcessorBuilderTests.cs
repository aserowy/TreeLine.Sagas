using Moq;
using System;
using System.Collections.Generic;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder
{
    public class SagaProcessorBuilderTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISagaServiceProvider> _mockSagaServiceProvider;

        public SagaProcessorBuilderTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaServiceProvider = _mockRepository.Create<ISagaServiceProvider>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaProcessorBuilder CreateSagaProcessorBuilder()
        {
            return new SagaProcessorBuilder(
                _mockSagaServiceProvider.Object);
        }

        [Fact]
        public void Build_NoStepsAdded_ThrowInvalidOperation()
        {
            // Arrange
            var sagaProcessorBuilder = CreateSagaProcessorBuilder();

            // Assert
            Assert.Throws<InvalidOperationException>(() => sagaProcessorBuilder.Build());
        }

        [Fact]
        public void Build_OneStepAdded_ProcessorContainsOneStep()
        {
            // Arrange
            var sagaProcessorBuilder = CreateSagaProcessorBuilder();
            var mockProcessor = _mockRepository.Create<ISagaProcessor>();

            _mockSagaServiceProvider
                .Setup(prvdr => prvdr.ResolveProcessor())
                .Returns(mockProcessor.Object);

            mockProcessor.Setup(prcsr => prcsr.AddSteps(It.Is<IList<ISagaStepConfiguration>>(stps => stps.Count == 1)));

            sagaProcessorBuilder.AddStep<SagaEvent01, SagaStep01Mock>();

            // Act
            sagaProcessorBuilder.Build();
        }

        [Fact]
        public void Build_TwoStepsAdded_ProcessorRetainsOrder()
        {
            // Arrange
            var sagaProcessorBuilder = CreateSagaProcessorBuilder();
            var mockProcessor = _mockRepository.Create<ISagaProcessor>();

            _mockSagaServiceProvider
                .Setup(prvdr => prvdr.ResolveProcessor())
                .Returns(mockProcessor.Object);

            var configurations = new List<ISagaStepConfiguration>();

            mockProcessor
                .Setup(prcsr => prcsr.AddSteps(It.Is<IList<ISagaStepConfiguration>>(stps => stps.Count == 2)))
                .Callback<IList<ISagaStepConfiguration>>(cnfgrtns => configurations.AddRange(cnfgrtns));

            sagaProcessorBuilder.AddStep<SagaEvent01, SagaStep01Mock>();
            sagaProcessorBuilder.AddStep<SagaEvent01, SagaStep02Mock>();

            // Act
            sagaProcessorBuilder.Build();

            // Assert
            Assert.Equal(typeof(SagaStep01Mock), configurations[0].SagaStepType);
            Assert.Equal(typeof(SagaStep02Mock), configurations[1].SagaStepType);
        }
    }
}