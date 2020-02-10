using System.Collections.Generic;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal interface IValidationRule
    {
        (IEnumerable<string>? Warnings, IEnumerable<ValidationException>? Exceptions) Validate(IEnumerable<ISagaProfileAnalyzer> analyzers);
    }
}