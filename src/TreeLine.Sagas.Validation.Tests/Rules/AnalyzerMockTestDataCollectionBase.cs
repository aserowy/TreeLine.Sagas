using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TreeLine.Sagas.Validation.Tests.Rules
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "abstract class name should end with base")]
    public abstract class AnalyzerMockTestDataCollectionBase : IEnumerable<object[]>
    {
        internal abstract object[]? GetReturnValuesByDataType(AnalyzerMockTestDataType type);

        public IEnumerator<object[]> GetEnumerator()
        {
            var resolver = new AnalyzerMockTestDataResolver();
            foreach (var kvp in resolver.Get())
            {
                var values = GetReturnValuesByDataType(kvp.Key);
                if (values is null)
                {
                    throw new KeyNotFoundException($"{GetType().Name} does not contain return values for test data of type {Enum.GetName(typeof(AnalyzerMockTestDataType), kvp.Key)}.");
                }

                var result = new List<object> { kvp.Value };
                result.AddRange(values);

                yield return result.ToArray();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
