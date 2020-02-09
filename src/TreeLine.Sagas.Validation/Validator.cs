using System;
using System.Collections.Generic;
using System.Linq;

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
            var result = new List<Exception>();
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

            Exception exception;
            if (result.Count.Equals(1))
            {
                exception = result.Single();
            }
            else
            {
                exception = new AggregateException($"Validation failed for current saga configuration.", result);
            }

            throw exception;
        }
    }
}
