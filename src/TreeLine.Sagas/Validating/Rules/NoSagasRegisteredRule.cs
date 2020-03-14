using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validating.Analyzing;

namespace TreeLine.Sagas.Validating.Rules
{
    internal sealed class NoSagasRegisteredRule : IValidationRule
    {
        public (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers)
        {
            if (analyzers.Any())
            {
                return (null, null);
            }

            return (new List<string> { "No sagas registered." }, null);
        }
    }
}
