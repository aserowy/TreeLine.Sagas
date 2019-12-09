using Moq;
using System;
using System.Collections.Generic;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder
{
    public class SagaProcessorBuilderTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISagaVersionFactory> _mockSagaVersionFactory;
        private readonly Mock<ISagaServiceProvider> _mockSagaServiceProvider;

        public SagaProcessorBuilderTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockSagaVersionFactory = _mockRepository.Create<ISagaVersionFactory>();
            _mockSagaServiceProvider = _mockRepository.Create<ISagaServiceProvider>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaProcessorBuilder CreateSagaProcessorBuilder()
        {
            return new SagaProcessorBuilder(
                _mockSagaVersionFactory.Object,
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

            _mockSagaVersionFactory
                .Setup(fctry => fctry.Create(It.IsAny<string>()))
                .Returns(new SagaVersion("1.0.0"));

            _mockSagaServiceProvider
                .Setup(prvdr => prvdr.ResolveProcessor())
                .Returns(mockProcessor.Object);

            mockProcessor.Setup(prcsr => prcsr.AddSteps(It.IsAny<ISagaVersion>(), It.Is<IList<ISagaStepConfiguration>>(stps => stps.Count == 1)));

            // Act
            sagaProcessorBuilder
                .AddVersion("1.0.0")
                .AddStep<SagaEvent01, SagaStep01Mock>();

            sagaProcessorBuilder.Build();
        }

        [Fact]
        public void Build_TwoStepsAdded_ProcessorRetainsOrder()
        {
            // Arrange
            var sagaProcessorBuilder = CreateSagaProcessorBuilder();
            var mockProcessor = _mockRepository.Create<ISagaProcessor>();

            _mockSagaVersionFactory
                .Setup(fctry => fctry.Create(It.IsAny<string>()))
                .Returns(new SagaVersion("1.0.0"));

            _mockSagaServiceProvider
                .Setup(prvdr => prvdr.ResolveProcessor())
                .Returns(mockProcessor.Object);

            var configurations = new List<ISagaStepConfiguration>();

            mockProcessor
                .Setup(prcsr => prcsr.AddSteps(It.IsAny<ISagaVersion>(), It.Is<IList<ISagaStepConfiguration>>(stps => stps.Count == 2)))
                .Callback<ISagaVersion, IList<ISagaStepConfiguration>>((_, cnfgrtns) => configurations.AddRange(cnfgrtns));

            // Act
            sagaProcessorBuilder
                .AddVersion("1.0.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent01, SagaStep02Mock>();

            sagaProcessorBuilder.Build();

            // Assert
            Assert.Equal(typeof(SagaStep01Mock), configurations[0].SagaStepType);
            Assert.Equal(typeof(SagaStep02Mock), configurations[1].SagaStepType);
        }
    }
}