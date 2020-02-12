using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests.Rules
{
    public class NoSagasRegisteredRuleTests
    {
        private NoSagasRegisteredRule CreateNoSagasRegisteredRule()
        {
            return new NoSagasRegisteredRule();
        }

        private class AnalyzerMockTestDataCollection : AnalyzerMockTestDataCollectionBase
        {
            internal override object[]? GetReturnValuesByDataType(AnalyzerMockTestDataType type)
            {
                return type switch
                {
                    AnalyzerMockTestDataType.NoSagasRegistered => new object[] { 1, 0 },
                    AnalyzerMockTestDataType.ValidAnalyzers => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.EqualAnalyzerIdentifier => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.EqualVersionIdentifier => new object[] { 0, 0 },
                    AnalyzerMockTestDataType.VersionWithoutSteps => new object[] { 0, 0 },
                    _ => null
                };
            }
        }

        [Theory]
        [ClassData(typeof(AnalyzerMockTestDataCollection))]
        internal void Validate(IList<ISagaProfileAnalyzer> analyzers, int countWarnings, int countExceptions)
        {
            // Arrange
            var noSagasRegisteredRule = CreateNoSagasRegisteredRule();

            // Act
            var (warnings, exceptions) = noSagasRegisteredRule.Validate(analyzers);

            // Assert
            Assert.Equal(countWarnings, warnings?.Count() ?? 0);
            Assert.Equal(countExceptions, exceptions?.Count() ?? 0);
        }
    }
}
