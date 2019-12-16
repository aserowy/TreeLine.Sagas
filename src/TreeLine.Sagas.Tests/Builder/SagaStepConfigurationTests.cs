using System;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Builder
{
    public class SagaStepConfigurationTests
    {
        [Fact]
        public void Ctor_VersionIsNull_ThrowsArgumentNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(null, 0, null));
        }

        [Fact]
        public void IsResponsible_EventTypeEqualPredicateNull_ReturnTrue()
        {
            // Arrange
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null);
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
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, _ => false);
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
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null);
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
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, _ => true);
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
            var sagaStepConfiguration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => sagaStepConfiguration.Create(null));
        }
    }
}