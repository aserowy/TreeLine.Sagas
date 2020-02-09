using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class VersionsWithoutStepsRule : IValidationRule
    {
        private readonly ILogger<VersionsWithoutStepsRule> _logger;

        public VersionsWithoutStepsRule(ILogger<VersionsWithoutStepsRule> logger)
        {
            _logger = logger;
        }

        public void Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            foreach (var analyzer in analyzers)
            {
                if (analyzer.VersionAnalyzer.Any(anlyzr => anlyzr.StepCount.Equals(0)))
                {
                    _logger.LogWarning($"{analyzer.ProfileName} has registered versions without configured steps.");
                }
            }
        }
    }
}
