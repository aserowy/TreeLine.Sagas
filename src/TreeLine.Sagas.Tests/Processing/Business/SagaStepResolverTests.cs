using System;
using System.Collections.Generic;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processing.Business;
using TreeLine.Sagas.ReferenceStore;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Processing.Business
{
    public class SagaStepResolverTests
    {
        [Fact]
        public void Create_RefsNullOneStepWithCorrectEvent_ReturnStep()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();
            var configuartion = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, null);
            var configurations = new List<ISagaStepConfiguration>
            {
                configuartion
            };

            // Act
            var result = func(sagaEvent, null, configurations);

            // Assert
            Assert.Same(configuartion, result);
        }

        [Fact]
        public void Create_RefsNullTwoStepsWithCorrectEvent_ReturnFirstStep()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();
            var configuartion = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, null);
            var configurations = new List<ISagaStepConfiguration>
            {
                configuartion,
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null)
            };

            // Act
            var result = func(sagaEvent, null, configurations);

            // Assert
            Assert.Same(configuartion, result);
        }

        [Fact]
        public void Create_RefsNullOneStepWithFalsePredicate_ThrowInvalidOperation()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, _ => false)
            };

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(sagaEvent, null, configurations));
        }

        [Fact]
        public void Create_RefsNullLastStepTruePredicate_ReturnLastStep()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var configuartion = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(
                new SagaVersion("1.0.0"),
                1,
                sgEvnt => sgEvnt.TransactionId == sagaEvent.TransactionId);

            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, _ => false),
                configuartion
            };

            // Act
            var result = func(sagaEvent, null, configurations);

            // Assert
            Assert.Same(configuartion, result);
        }

        [Fact]
        public void Create_RefsNullOneStepWithDifferentEvent_ThrowInvalidOperation()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent02();

            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, _ => false)
            };

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(sagaEvent, null, configurations));
        }

        [Fact]
        public void Create_RefWithoutMatchingReferenceId_HandledLikeRefsNull()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var references = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.0.0"), 0, SagaMessageType.Command, Guid.NewGuid(), sagaEvent.TransactionId)
            };

            var configuration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, null);
            var configurations = new List<ISagaStepConfiguration>
            {
                configuration,
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null)
            };

            // Act
            var result = func(sagaEvent, references, configurations);

            // Assert
            Assert.Same(configuration, result);
        }

        [Fact]
        public void Create_RefWithMultipleReferenceIdOneMatching_UseFirstReference()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var references = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.0.0"), 1, SagaMessageType.Command, Guid.NewGuid(), sagaEvent.TransactionId),
                new SagaReference(new SagaVersion("1.0.0"), 0, SagaMessageType.Command, sagaEvent.ProcessId, sagaEvent.TransactionId)
            };

            var configuration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null);
            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, null),
                configuration
            };

            // Act
            var result = func(sagaEvent, references, configurations);

            // Assert
            Assert.Same(configuration, result);
        }

        [Fact]
        public void Create_RefWithoutMatchingTransactionId_ThrowInvalidOperation()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var references = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.0.0"), 0, SagaMessageType.Command, sagaEvent.ProcessId, Guid.NewGuid())
            };

            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, null),
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null)
            };

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(sagaEvent, references, configurations));
        }

        [Fact]
        public void Create_RefWithMultipleMatchingTransactionId_UseLastReference()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var references = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.0.0"), 0, SagaMessageType.Command, sagaEvent.ProcessId, sagaEvent.TransactionId),
                new SagaReference(new SagaVersion("1.0.0"), 1, SagaMessageType.Command, sagaEvent.ProcessId, sagaEvent.TransactionId)
            };

            var configuration = new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 2, null);
            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, null),
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, null),
                configuration,
            };

            // Act
            var result = func(sagaEvent, references, configurations);

            // Assert
            Assert.Same(configuration, result);
        }

        [Fact]
        public void Create_RefWithMatchingTransactionIdButFailingPredicate_ThrowInvalidOperation()
        {
            // Arrange
            var func = new SagaStepResolver().Create();
            var sagaEvent = new SagaEvent01();

            var references = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.0.0"), 1, SagaMessageType.Command, sagaEvent.ProcessId, sagaEvent.TransactionId)
            };

            var configurations = new List<ISagaStepConfiguration>
            {
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 0, _ => false),
                new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(new SagaVersion("1.0.0"), 1, _ => false)
            };

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(sagaEvent, references, configurations));
        }
    }
}