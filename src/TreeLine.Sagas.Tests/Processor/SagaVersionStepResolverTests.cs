using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Processor;
using TreeLine.Sagas.Processor.Business;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Versioning;
using Xunit;

namespace TreeLine.Sagas.Tests.Processor
{
    public class SagaVersionStepResolverTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ISagaVersionResolver> _mockSagaVersionResolver;
        private readonly Mock<ISagaStepResolver> _mockSagaStepResolver;

        public SagaVersionStepResolverTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSagaVersionResolver = _mockRepository.Create<ISagaVersionResolver>();
            _mockSagaStepResolver = _mockRepository.Create<ISagaStepResolver>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        private SagaVersionStepResolver CreateSagaVersionStepResolver()
        {
            return new SagaVersionStepResolver(
                new NullSagaEventStore(),
                _mockSagaVersionResolver.Object,
                _mockSagaStepResolver.Object);
        }

        [Fact]
        public async Task ResolveAsync_EventStoreReturnsNull_FunctionsAreCalled()
        {
            // Arrange
            var sagaVersionStepResolver = CreateSagaVersionStepResolver();
            var sagaEvent = new SagaEvent01();

            var sagaVersion = new SagaVersion("1.0.0");
            var mockConfiguration01 = _mockRepository.Create<ISagaStepConfiguration>();
            var mockConfiguration02 = _mockRepository.Create<ISagaStepConfiguration>();

            var versions = new Dictionary<ISagaVersion, IList<ISagaStepConfiguration>>
            {
                {
                    sagaVersion,
                    new List<ISagaStepConfiguration>
                    {
                        mockConfiguration01.Object,
                        mockConfiguration02.Object
                    }
                }
            };

            _mockSagaVersionResolver
                .Setup(vrsnRslvr => vrsnRslvr.Create())
                .Returns((_, versions) => versions.Single().Value);

            _mockSagaStepResolver
                .Setup(stpRslvr => stpRslvr.Create())
                .Returns((_, __, configurations) => configurations[0]);

            // Act
            var result = await sagaVersionStepResolver
                .ResolveAsync(sagaEvent, versions)
                .ConfigureAwait(false);

            // Assert
            Assert.Same(mockConfiguration01.Object, result);
        }
    }
}