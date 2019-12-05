using Moq;
using System;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Tests.Mocks;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder
{
    public class SagaStepConfigurationTests
    {
        [Fact]
        public void IsResponsible_EventTypeEqualPredicateNull_ReturnTrue()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent, SagaStep01Mock>(null);
            var sagaEvent = new SagaEvent();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsResponsible_EventTypeEqualPredicateReturnsFalse_ReturnFalse()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent, SagaStep01Mock>(_ => false);
            var sagaEvent = new SagaEvent();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsResponsible_EventTypeNotEqualPredicateNull_ReturnFalse()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent, SagaStep01Mock>(null);
            var sagaEvent = new DifferentSagaEvent();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsResponsible_EventTypeNotEqualPredicateReturnsTrue_ReturnFalse()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent, SagaStep01Mock>(_ => true);
            var sagaEvent = new DifferentSagaEvent();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_ServiceProviderReturnsStep_ReturnsSameStep()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent, SagaStep01Mock>(null);

            var mockSagaStep = new SagaStep01Mock();

            var mockRepository = new MockRepository(MockBehavior.Strict);
            var mockSagaServiceProvider = mockRepository.Create<ISagaServiceProvider>();
            mockSagaServiceProvider
                .Setup(prvdr => prvdr.Resolve<SagaStep01Mock>())
                .Returns(mockSagaStep);

            // Act
            var result = sagaStepConfiguration.Create(mockSagaServiceProvider.Object);

            // Assert
            Assert.Same(mockSagaStep, result);
        }

        [Fact]
        public void Create_ServiceProviderIsNull_ThrowsArgumentNull()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent, SagaStep01Mock>(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => sagaStepConfiguration.Create(null));
        }

        private sealed class DifferentSagaEvent : ISagaEvent
        {
            public Guid ReferenceId => Guid.Empty;

            public Guid TransactionId => Guid.Empty;
        }
    }
}