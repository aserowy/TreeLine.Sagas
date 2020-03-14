using System.Collections.Generic;
using TreeLine.Sagas.ReferenceStore;
using TreeLine.Sagas.Validating.Analyzing;

namespace TreeLine.Sagas.Validating.Rules
{
    internal sealed class MultipleVersionsWithoutReferenceStoreRule : IValidationRule
    {
        private readonly IReferenceStore _store;

        public MultipleVersionsWithoutReferenceStoreRule(IReferenceStore store)
        {
            _store = store;
        }

        public (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            if (!(_store is EmptyReferenceStore))
            {
                return (null, null);
            }

            var warnings = new List<string>();
            foreach (var analyzer in analyzers)
            {
                if (analyzer.VersionAnalyzer.Count > 1)
                {
                    warnings.Add($"{analyzer.ProfileName} has multiple registered versions without a registered event store.");
                }
            }

            return (warnings, null);
        }
    }
}
