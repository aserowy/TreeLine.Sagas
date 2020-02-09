using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Rules.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class VersionsWithoutStepsRule : IValidationRule
    {
        private readonly ISagaProfileAnalyzerFactory _analyzerFactory;
        private readonly IEnumerable<ISagaProfile> _profiles;
        private readonly ILogger<VersionsWithoutStepsRule> _logger;

        public VersionsWithoutStepsRule(
            ISagaProfileAnalyzerFactory analyzerFactory,
            IEnumerable<ISagaProfile> profiles,
            ILogger<VersionsWithoutStepsRule> logger)
        {
            _analyzerFactory = analyzerFactory;
            _profiles = profiles;
            _logger = logger;
        }

        public void Validate()
        {
            foreach (var profile in _profiles)
            {
                var analyzer = _analyzerFactory.Create();
                profile.Configure(analyzer);

                if (analyzer.VersionAnalyzer.Any(anlyzr => anlyzr.StepCount.Equals(0)))
                {
                    _logger.LogWarning($"{profile.GetType().Name} has registered versions without configured steps.");
                }
            }
        }
    }
}
