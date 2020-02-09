using System.Collections.Generic;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Rules
{
    internal interface IValidationRule
    {
        void Validate(IEnumerable<ISagaProfileAnalyzer> analyzers);
    }
}