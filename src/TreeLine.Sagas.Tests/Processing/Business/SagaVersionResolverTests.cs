using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processing.Business;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Processing.Business
{
    public class SagaVersionResolverTests
    {
        [Fact]
        public void Create_NoRefsOneVersion_ReturnStepWithVersion()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();

            // Act
            var result = func(null, CreateVersions("1.0.0"));

            // Assert
            Assert.Equal("1.0.0", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_NoRefsMultipleVersions_ReturnStepWithHighestVersion()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();

            // Act
            var result = func(null, CreateVersions("1.0.0", "0.0.9", "2.12.1", "12.5.99", "1.4.5"));

            // Assert
            Assert.Equal("12.5.99", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_RefsVersionMatchesExact_ReturnStepWithHighestVersion()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.4.5"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid())
            };

            // Act
            var result = func(refs, CreateVersions("1.0.0", "0.0.9", "2.10.1", "12.5.99", "1.4.5"));

            // Assert
            Assert.Equal("1.4.5", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_RefsVersionMatchesOnMajorMinor_ReturnStepWithHighestPatch()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.4.1"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid())
    };

            // Act
            var result = func(refs, CreateVersions("1.0.0", "0.0.9", "2.10.1", "12.5.99", "1.4.5"));

            // Assert
            Assert.Equal("1.4.5", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_RefsVersionMatchesOnMajorMinorHigherPatch_ReturnStepWithHighestPatch()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.4.99"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid())
        };

            // Act
            var result = func(refs, CreateVersions("1.0.0", "0.0.9", "2.10.1", "12.5.99", "1.4.5"));

            // Assert
            Assert.Equal("1.4.5", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_RefsVersionMatchesNoMajorMinor_ThrowInvalidOperation()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.3.99"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid())
        };

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(refs, CreateVersions("1.0.0", "0.0.9", "2.10.1", "12.5.99", "1.4.5")));
        }

        [Fact]
        public void Create_RefsWithMultipleVersionsOnPatch_ReturnStepWithHighestPatch()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var referenceId = Guid.NewGuid();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("1.4.1"), 0, SagaMessageType.Command,  referenceId, Guid.NewGuid()),
                new SagaReference(new SagaVersion("1.4.99"), 0, SagaMessageType.Command,  referenceId, Guid.NewGuid()),
                new SagaReference(new SagaVersion("1.4.15"), 0, SagaMessageType.Command,  referenceId, Guid.NewGuid())
        };

            // Act
            var result = func(refs, CreateVersions("1.4.5"));

            // Assert
            Assert.Equal("1.4.5", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_RefsWithDifferentMajorVersions_ReturnStepWithHighestVersion()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var referenceId = Guid.NewGuid();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("2.4.1"), 0, SagaMessageType.Command,  referenceId, Guid.NewGuid()),
                new SagaReference(new SagaVersion("1.4.99"), 0, SagaMessageType.Command,  referenceId, Guid.NewGuid()),
                new SagaReference(new SagaVersion("1.4.15"), 0, SagaMessageType.Command,  referenceId, Guid.NewGuid())
            };

            // Act
            var result = func(refs, CreateVersions("2.4.5"));

            // Assert
            Assert.Equal("2.4.5", result.Single().Version.ToString());
        }

        [Fact]
        public void Create_NoRefsNoVersions_ThrowInvalidOperation()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(null, null));
        }

        private static IDictionary<ISagaVersion, IList<ISagaStepConfiguration>> CreateVersions(params string[] versions)
        {
            var result = new Dictionary<ISagaVersion, IList<ISagaStepConfiguration>>();
            foreach (var version in versions)
            {
                var sagaVersion = new SagaVersion(version);

                result.Add(sagaVersion, new List<ISagaStepConfiguration> { new SagaStepConfiguration<SagaEvent01, SagaStep01Mock>(sagaVersion, 0, null) });
            }

            return result;
        }

        [Fact]
        public void Create_RefsWithMultipleReferenceIds_ThrowsInvalidOperation()
        {
            // Arrange
            var func = new SagaVersionResolver().Create();
            var refs = new List<ISagaReference>
            {
                new SagaReference(new SagaVersion("2.4.1"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid()),
                new SagaReference(new SagaVersion("2.4.1"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid()),
                new SagaReference(new SagaVersion("2.4.1"), 0, SagaMessageType.Command,  Guid.NewGuid(), Guid.NewGuid())
            };

            // Assert
            Assert.Throws<InvalidOperationException>(() => func(refs, CreateVersions("2.4.5")));
        }
    }
}