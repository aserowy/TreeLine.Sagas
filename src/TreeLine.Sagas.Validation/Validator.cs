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
        private readonly ILoggerAdapter<Validator> _logger;

        public Validator(
            ISagaProfileAnalyzerResolver analyzerResolver,
            IEnumerable<IValidationRule> rules,
            ILoggerAdapter<Validator> logger)
        {
            _analyzerResolver = analyzerResolver;
            _rules = rules;
            _logger = logger;
        }

        public void Validate()
        {
            var analyzer = _analyzerResolver.Get();

            var result = new List<ValidationException>();
            foreach (var rule in _rules)
            {
                try
                {
                    var (warnings, exceptions) = rule.Validate(analyzer);
                    result.AddRange(exceptions ?? Enumerable.Empty<ValidationException>());

                    if (warnings is null)
                    {
                        continue;
                    }

                    foreach (var warning in warnings)
                    {
                        _logger.LogWarning(warning);
                    }
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
