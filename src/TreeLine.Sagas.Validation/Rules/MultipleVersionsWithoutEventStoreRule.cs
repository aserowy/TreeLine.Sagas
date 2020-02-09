using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Validation.Rules.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class MultipleVersionsWithoutEventStoreRule : IValidationRule
    {
        private readonly ISagaEventStore _store;
        private readonly ISagaProfileAnalyzerFactory _analyzerFactory;
        private readonly IEnumerable<ISagaProfile> _profiles;
        private readonly ILogger<MultipleVersionsWithoutEventStoreRule> _logger;

        public MultipleVersionsWithoutEventStoreRule(
            ISagaEventStore store,
            ISagaProfileAnalyzerFactory analyzerFactory,
            IEnumerable<ISagaProfile> profiles,
            ILogger<MultipleVersionsWithoutEventStoreRule> logger)
        {
            _store = store;
            _analyzerFactory = analyzerFactory;
            _profiles = profiles;
            _logger = logger;
        }

        public void Validate()
        {
            if (!(_store is NullSagaEventStore))
            {
                return;
            }

            foreach (var profile in _profiles)
            {
                var analyzer = _analyzerFactory.Create();
                profile.Configure(analyzer);

                if (analyzer.VersionAnalyzer.Count > 1)
                {
                    _logger.LogWarning($"{profile.GetType().Name} has multiple registered versions without a registered event store.");
                }
            }
        }
    }
}
