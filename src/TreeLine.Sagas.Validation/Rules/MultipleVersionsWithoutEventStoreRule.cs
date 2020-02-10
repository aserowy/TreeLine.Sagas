using System.Collections.Generic;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class MultipleVersionsWithoutEventStoreRule : IValidationRule
    {
        private readonly ISagaEventStore _store;

        public MultipleVersionsWithoutEventStoreRule(ISagaEventStore store)
        {
            _store = store;
        }

        public (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            if (!(_store is NullSagaEventStore))
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
