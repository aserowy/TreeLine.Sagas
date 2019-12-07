using System;
using TreeLine.Sagas.Builder;
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
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(null);
            var sagaEvent = new SagaEvent01();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsResponsible_EventTypeEqualPredicateReturnsFalse_ReturnFalse()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(_ => false);
            var sagaEvent = new SagaEvent01();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsResponsible_EventTypeNotEqualPredicateNull_ReturnFalse()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(null);
            var sagaEvent = new SagaEvent02();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsResponsible_EventTypeNotEqualPredicateReturnsTrue_ReturnFalse()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(_ => true);
            var sagaEvent = new SagaEvent02();

            // Act
            var result = sagaStepConfiguration.IsResponsible(sagaEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_ServiceProviderIsNull_ThrowsArgumentNull()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => sagaStepConfiguration.Create(null));
        }
    }
}