using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
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
