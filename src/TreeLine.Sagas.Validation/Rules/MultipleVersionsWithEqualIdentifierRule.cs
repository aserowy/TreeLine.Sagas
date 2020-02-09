using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Validation.Rules.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class MultipleVersionsWithEqualIdentifierRule : IValidationRule
    {
        private readonly ISagaEventStore _store;
        private readonly ISagaProfileAnalyzerFactory _analyzerFactory;
        private readonly IEnumerable<ISagaProfile> _profiles;

        public MultipleVersionsWithEqualIdentifierRule(
            ISagaEventStore store,
            ISagaProfileAnalyzerFactory analyzerFactory,
            IEnumerable<ISagaProfile> profiles)
        {
            _store = store;
            _analyzerFactory = analyzerFactory;
            _profiles = profiles;
        }

        public void Validate()
        {
            if (!(_store is NullSagaEventStore))
            {
                return;
            }

            var exceptions = new List<ValidationException>();
            foreach (var profile in _profiles)
            {
                var analyzer = _analyzerFactory.Create();
                profile.Configure(analyzer);

                var analysis = analyzer
                    .VersionAnalyzer
                    .GroupBy(anlyzr => anlyzr.Version)
                    .Where(grp => grp.Count() > 1);

                if (analysis.Any())
                {
                    exceptions.Add(new ValidationException($"{profile.GetType().Name} has multiple Versions with same identifier."));
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
