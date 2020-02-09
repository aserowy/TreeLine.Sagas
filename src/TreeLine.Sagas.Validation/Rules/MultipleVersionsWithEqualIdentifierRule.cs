using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class MultipleVersionsWithEqualIdentifierRule : IValidationRule
    {
        private readonly ISagaEventStore _store;

        public MultipleVersionsWithEqualIdentifierRule(ISagaEventStore store)
        {
            _store = store;
        }

        public void Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            if (!(_store is NullSagaEventStore))
            {
                return;
            }

            var exceptions = new List<ValidationException>();
            foreach (var analyzer in analyzers)
            {
                var analysis = analyzer
                    .VersionAnalyzer
                    .GroupBy(anlyzr => anlyzr.Version)
                    .Where(grp => grp.Count() > 1);

                if (analysis.Any())
                {
                    exceptions.Add(new ValidationException($"{analyzer.ProfileName} has multiple Versions with same identifier."));
                }
            }

            if (exceptions.Any())
            {
                if (exceptions.Count.Equals(1))
                {
                    throw exceptions.Single();
                }
                else
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}
