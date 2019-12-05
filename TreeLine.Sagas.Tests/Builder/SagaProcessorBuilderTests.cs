using Moq;
using System;
using TreeLine.Sagas.Builder;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder
{
    public class SagaProcessorBuilderTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ISagaServiceProvider> mockSagaServiceProvider;

        public SagaProcessorBuilderTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSagaServiceProvider = this.mockRepository.Create<ISagaServiceProvider>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private SagaProcessorBuilder CreateSagaProcessorBuilder()
        {
            return new SagaProcessorBuilder(
                this.mockSagaServiceProvider.Object);
        }

        [Fact]
        public void AddStep_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sagaProcessorBuilder = this.CreateSagaProcessorBuilder();
            Predicate<TEvent>? customValidation = null;

            // Act
            var result = sagaProcessorBuilder.AddStep(
                customValidation);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Build_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sagaProcessorBuilder = this.CreateSagaProcessorBuilder();

            // Act
            var result = sagaProcessorBuilder.Build();

            // Assert
            Assert.True(false);
        }
    }
}
