using Moq;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Validation.Analyzing;
using TreeLine.Sagas.Validation.Rules;
using Xunit;

namespace TreeLine.Sagas.Validation.Tests
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
        public void Validate_WarningsReturned_LoggerCalled()
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

        private IEnumerable<Mock<IValidationRule>> GetRuleMocks(int count)
        {
            var result = new List<Mock<IValidationRule>>();
            for (int i = 0; i < count; i++)
            {
                var mock = _mockRepository.Create<IValidationRule>();
                mock
                    .Setup(mck => mck.Validate(It.IsAny<IEnumerable<ISagaProfileAnalyzer>>()))
                    .Returns((null, null));

                result.Add(mock);
            }

            return result;
        }
    }
}
