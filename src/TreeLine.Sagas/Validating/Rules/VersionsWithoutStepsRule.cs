using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validating.Analyzing;

namespace TreeLine.Sagas.Validating.Rules
{
    internal sealed class VersionsWithoutStepsRule : IValidationRule
    {
        public (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            var warnings = new List<string>();
            foreach (var analyzer in analyzers)
            {
                if (analyzer.VersionAnalyzer.Any(anlyzr => anlyzr.StepCount.Equals(0)))
                {
                    warnings.Add($"{analyzer.ProfileName} has registered versions without configured steps.");
                }
            }

            return (warnings, null);
        }
    }
}
