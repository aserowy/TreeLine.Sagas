using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests.Rules
{
    public class MultipleVersionsWithEqualIdentifierRuleTests
    {
        private MultipleVersionsWithEqualIdentifierRule CreateMultipleVersionsWithEqualIdentifierRule()
        {
            return new MultipleVersionsWithEqualIdentifierRule();
        }

        private class AnalyzerMockTestDataCollection : AnalyzerMockTestDataCollectionBase
        {
            internal override object[]? GetReturnValuesByDataType(AnalyzerMockTestDataType type)
            {
                return type switch
                {
                    AnalyzerMockTestDataType.NoSagasRegistered => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.ValidAnalyzers => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.EqualAnalyzerIdentifier => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.EqualVersionIdentifier => new object[] { 0, 2 },
                    _ => null
                };
            }
        }

        [Theory]
        [ClassData(typeof(AnalyzerMockTestDataCollection))]
        internal void Validate(IList<ISagaProfileAnalyzer> analyzers, int countWarnings, int countExceptions)
        {
            // Arrange
            var multipleVersionsWithEqualIdentifierRule = CreateMultipleVersionsWithEqualIdentifierRule();

            // Act
            var (warnings, exceptions) = multipleVersionsWithEqualIdentifierRule.Validate(analyzers);

            // Assert
            Assert.Equal(countWarnings, warnings?.Count() ?? 0);
            Assert.Equal(countExceptions, exceptions?.Count() ?? 0);
        }

    }
}
