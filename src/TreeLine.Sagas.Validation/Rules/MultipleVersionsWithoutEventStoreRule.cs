using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class MultipleVersionsWithoutEventStoreRule : IValidationRule
    {
        private readonly ISagaEventStore _store;
        private readonly ILogger<MultipleVersionsWithoutEventStoreRule> _logger;

        public MultipleVersionsWithoutEventStoreRule(
            ISagaEventStore store,
            ILogger<MultipleVersionsWithoutEventStoreRule> logger)
        {
            _store = store;
            _logger = logger;
        }

        public void Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            if (!(_store is NullSagaEventStore))
            {
                return;
            }

            foreach (var analyzer in analyzers)
            {
                if (analyzer.VersionAnalyzer.Count > 1)
                {
                    _logger.LogWarning($"{analyzer.ProfileName} has multiple registered versions without a registered event store.");
                }
            }
        }
    }
}
