using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Validating;
using TreeLine.Sagas.Validating.Analyzing;
using TreeLine.Sagas.Validating.Rules;
using Xunit;

namespace TreeLine.Sagas.Tests.Validating
{
    public class ValidatorTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ISagaProfileAnalyzerResolver> _mockSagaProfileAnalyzerResolver;
        private readonly Mock<IEnumerable<IValidationRule>> _mockEnumerable;
        private readonly Mock<ILoggerAdapter<Validator>> _mockLogger;

        public ValidatorTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaProfileAnalyzerResolver = _mockRepository.Create<ISagaProfileAnalyzerResolver>();
            _mockEnumerable = _mockRepository.Create<IEnumerable<IValidationRule>>();
            _mockLogger = _mockRepository.Create<ILoggerAdapter<Validator>>();
        }

        private Validator CreateValidator()
        {
            return new Validator(
                _mockSagaProfileAnalyzerResolver.Object,
                _mockEnumerable.Object,
                _mockLogger.Object);
        }

        private IEnumerable<Mock<IValidationRule>> GetRuleMocks(int count, IEnumerable<ISagaProfileAnalyzer>? analyzers = null)
        {
            var result = new List<Mock<IValidationRule>>();
            for (int i = 0; i < count; i++)
            {
                var mock = _mockRepository.Create<IValidationRule>();

                if (analyzers is null)
                {
                    mock
                        .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                        .Returns((null, null));
                }
                else
                {
                    mock
                        .Setup(mck => mck.Validate(analyzers))
                        .Returns((null, null));
                }

                result.Add(mock);
            }

            return result;
        }

        [Fact]
        public void Validate_AnalyzersReturned_RulesAreCalledWithAnalyzers()
        {
            // Arrange
            var validator = CreateValidator();

            var analyzers = new List<ISagaProfileAnalyzer>
            {
                new SagaProfileAnalyzerMock("test")
            };

            _mockSagaProfileAnalyzerResolver
                .Setup(mck => mck.Get())
                .Returns(analyzers);

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(GetRuleMocks(2, analyzers).Select(mck => mck.Object).GetEnumerator());

            // Act
            validator.Validate();

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Validate_NoAnalyzersReturned_RulesAreCalled()
        {
            // Arrange
            var validator = CreateValidator();
            _mockSagaProfileAnalyzerResolver
                .Setup(mck => mck.Get())
                .Returns(Enumerable.Empty<ISagaProfileAnalyzer>());

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(GetRuleMocks(2).Select(mck => mck.Object).GetEnumerator());

            // Act
            validator.Validate();

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Validate_SingleExceptionReturned_ThrowValidationException()
        {
            // Arrange
            var validator = CreateValidator();
            _mockSagaProfileAnalyzerResolver
                .Setup(mck => mck.Get())
                .Returns(Enumerable.Empty<ISagaProfileAnalyzer>());

            var exceptions = new List<ValidationException> { new ValidationException() };

            var ruleMocks = GetRuleMocks(1);
            var ruleMock = ruleMocks.Single();
            ruleMock.Reset();
            ruleMock
                .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                .Returns((null, exceptions));

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(ruleMocks.Select(mck => mck.Object).GetEnumerator());

            // Assert
            Assert.Throws<ValidationException>(() => validator.Validate());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Validate_MultipleExceptionReturned_ThrowAggregateException()
        {
            // Arrange
            var validator = CreateValidator();
            _mockSagaProfileAnalyzerResolver
                .Setup(mck => mck.Get())
                .Returns(Enumerable.Empty<ISagaProfileAnalyzer>());

            var exceptions01 = new List<ValidationException> { new ValidationException() };

            var ruleMocks = GetRuleMocks(2);
            var ruleMock01 = ruleMocks.First();
            ruleMock01.Reset();
            ruleMock01
                .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                .Returns((null, exceptions01));

            var exceptions02 = new List<ValidationException> { new ValidationException() };

            var ruleMock02 = ruleMocks.Last();
            ruleMock02.Reset();
            ruleMock02
                .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                .Returns((null, exceptions02));

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(ruleMocks.Select(mck => mck.Object).GetEnumerator());

            // Assert
            var exception = Assert.Throws<AggregateException>(() => validator.Validate());
            Assert.Equal(2, exception.InnerExceptions.Count);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Validate_OneRuleReturnsWarnings_LoggerCalled()
        {
            // Arrange
            var validator = CreateValidator();
            _mockSagaProfileAnalyzerResolver
                .Setup(mck => mck.Get())
                .Returns(Enumerable.Empty<ISagaProfileAnalyzer>());

            var warnings = new List<string>
            {
                "W1",
                "W2"
            };

            var ruleMocks = GetRuleMocks(1);
            var ruleMock = ruleMocks.Single();
            ruleMock.Reset();
            ruleMock
                .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                .Returns((warnings, null));

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(ruleMocks.Select(mck => mck.Object).GetEnumerator());

            _mockLogger.Setup(mck => mck.LogWarning("W1"));
            _mockLogger.Setup(mck => mck.LogWarning("W2"));

            // Act
            validator.Validate();

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Validate_MultipleRulesReturnsWarnings_LoggerCalled()
        {
            // Arrange
            var validator = CreateValidator();
            _mockSagaProfileAnalyzerResolver
                .Setup(mck => mck.Get())
                .Returns(Enumerable.Empty<ISagaProfileAnalyzer>());

            var warnings01 = new List<string> { "W1", "W2" };

            var ruleMocks = GetRuleMocks(2);
            var ruleMock01 = ruleMocks.First();
            ruleMock01.Reset();
            ruleMock01
                .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                .Returns((warnings01, null));

            var warnings02 = new List<string> { "W3", "W4" };

            var ruleMock02 = ruleMocks.Last();
            ruleMock02.Reset();
            ruleMock02
                .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                .Returns((warnings02, null));

            _mockEnumerable
                .Setup(mck => mck.GetEnumerator())
                .Returns(ruleMocks.Select(mck => mck.Object).GetEnumerator());

            _mockLogger.Setup(mck => mck.LogWarning("W1"));
            _mockLogger.Setup(mck => mck.LogWarning("W2"));
            _mockLogger.Setup(mck => mck.LogWarning("W3"));
            _mockLogger.Setup(mck => mck.LogWarning("W4"));

            // Act
            validator.Validate();

            // Assert
            _mockRepository.VerifyAll();
        }
    }
}
