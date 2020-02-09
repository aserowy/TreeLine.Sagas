using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Rules;

namespace TreeLine.Sagas.Validation
{
    internal sealed class Validator
    {
        private readonly IEnumerable<IValidationRule> _rules;

        public Validator(IEnumerable<IValidationRule> rules)
        {
            _rules = rules;
        }

        public void Validate()
        {
            var result = new List<ValidationException>();
            foreach (var rule in _rules)
            {
                try
                {
                    rule.Validate();
                }
                catch (ValidationException e)
                {
                    result.Add(e);
                }
            }

            if (result.Count.Equals(0))
            {
                return;
            }

            if (result.Count.Equals(1))
            {
                throw result.Single();
            }
            else
            {
                throw new AggregateException($"Validation failed for current saga configuration.", result);
            }
        }
    }
}
