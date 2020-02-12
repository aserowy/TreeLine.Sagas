using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal sealed class MultipleVersionsWithEqualIdentifierRule : IValidationRule
    {
        public (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
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

            return (null, exceptions);
        }
    }
}
