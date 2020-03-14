using System.Collections.Generic;
using TreeLine.Sagas.Validating.Analyzing;

namespace TreeLine.Sagas.Validating.Rules
{
    internal interface IValidationRule
    {
        (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers);
    }
}