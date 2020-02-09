using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;

namespace TreeLine.Sagas.Validation
{
    public interface IValidator
    {
        void Validate();
    }

    internal sealed class Validator : IValidator
    {
        private readonly ISagaProfileAnalyzerResolver _analyzerResolver;
        private readonly IEnumerable<IValidationRule> _rules;

        public Validator(
            ISagaProfileAnalyzerResolver analyzerResolver,
            IEnumerable<IValidationRule> rules)
        {
            _analyzerResolver = analyzerResolver;
            _rules = rules;
        }

        public void Validate()
        {
            var analyzer = _analyzerResolver.Get();

            var result = new List<ValidationException>();
            foreach (var rule in _rules)
            {
                try
                {
                    rule.Validate(analyzer);
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
