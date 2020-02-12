using Moq;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests.Rules
{
    public class MultipleVersionsWithoutEventStoreRuleTests
    {
        private class NullSagaEventStoreAnalyzerMockTestDataCollection : AnalyzerMockTestDataCollectionBase
        {
            internal override object[]? GetReturnValuesByDataType(AnalyzerMockTestDataType type)
            {
                return type switch
                {
                    AnalyzerMockTestDataType.NoSagasRegistered => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.ValidAnalyzers => new object[] { 2, 0 },
                    AnalyzerMockTestDataType.EqualAnalyzerIdentifier => new object[] { 2, 0 },
                    AnalyzerMockTestDataType.EqualVersionIdentifier => new object[] { 2, 0 },
                    AnalyzerMockTestDataType.VersionWithoutSteps => new object[] { 2, 0 },
                    _ => null
                };
            }
        }

        [Theory]
        [ClassData(typeof(NullSagaEventStoreAnalyzerMockTestDataCollection))]
        internal void Validate_NullSagaEventStore(IList<ISagaProfileAnalyzer> analyzers, int countWarnings, int countExceptions)
        {
            // Arrange
            var multipleVersionsWithoutEventStoreRule = new MultipleVersionsWithoutEventStoreRule(new NullSagaEventStore());

            // Act
            var (warnings, exceptions) = multipleVersionsWithoutEventStoreRule.Validate(analyzers);

            // Assert
            Assert.Equal(countWarnings, warnings?.Count() ?? 0);
            Assert.Equal(countExceptions, exceptions?.Count() ?? 0);
        }

        private class SagaEventStoreMockAnalyzerMockTestDataCollection : AnalyzerMockTestDataCollectionBase
        {
            internal override object[]? GetReturnValuesByDataType(AnalyzerMockTestDataType type)
            {
                return type switch
                {
                    AnalyzerMockTestDataType.NoSagasRegistered => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.ValidAnalyzers => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.EqualAnalyzerIdentifier => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.EqualVersionIdentifier => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.VersionWithoutSteps => new object[] { 0, 0 },
                    _ => null
                };
            }
        }

        [Theory]
        [ClassData(typeof(SagaEventStoreMockAnalyzerMockTestDataCollection))]
        internal void Validate_SagaEventStoreMock(IList<ISagaProfileAnalyzer> analyzers, int countWarnings, int countExceptions)
        {
            // Arrange
            var multipleVersionsWithoutEventStoreRule = new MultipleVersionsWithoutEventStoreRule(new Mock<ISagaEventStore>().Object);

            // Act
            var (warnings, exceptions) = multipleVersionsWithoutEventStoreRule.Validate(analyzers);

            // Assert
            Assert.Equal(countWarnings, warnings?.Count() ?? 0);
            Assert.Equal(countExceptions, exceptions?.Count() ?? 0);
        }
    }
}
