using Moq;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.ReferenceStore;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests.Rules
{
    public class MultipleVersionsWithoutReferenceStoreRuleTests
    {
        private class NullSagaReferenceStoreAnalyzerMockTestDataCollection : AnalyzerMockTestDataCollectionBase
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
        [ClassData(typeof(NullSagaReferenceStoreAnalyzerMockTestDataCollection))]
        internal void Validate_NullSagaReferenceStore(IList<ISagaProfileAnalyzer> analyzers, int countWarnings, int countExceptions)
        {
            // Arrange
            var multipleVersionsWithoutReferenceStoreRule = new MultipleVersionsWithoutReferenceStoreRule(new EmptyReferenceStore());

            // Act
            var (warnings, exceptions) = multipleVersionsWithoutReferenceStoreRule.Validate(analyzers);

            // Assert
            Assert.Equal(countWarnings, warnings?.Count() ?? 0);
            Assert.Equal(countExceptions, exceptions?.Count() ?? 0);
        }

        private class SagaReferenceStoreMockAnalyzerMockTestDataCollection : AnalyzerMockTestDataCollectionBase
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
        [ClassData(typeof(SagaReferenceStoreMockAnalyzerMockTestDataCollection))]
        internal void Validate_SagaReferenceStoreMock(IList<ISagaProfileAnalyzer> analyzers, int countWarnings, int countExceptions)
        {
            // Arrange
            var multipleVersionsWithoutReferenceStoreRule = new MultipleVersionsWithoutReferenceStoreRule(new Mock<IReferenceStore>().Object);

            // Act
            var (warnings, exceptions) = multipleVersionsWithoutReferenceStoreRule.Validate(analyzers);

            // Assert
            Assert.Equal(countWarnings, warnings?.Count() ?? 0);
            Assert.Equal(countExceptions, exceptions?.Count() ?? 0);
        }
    }
}
