using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Messaging;
using TreeLine.Sagas.Processing.Business;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Processing
{
    internal interface ISagaVersionStepResolver
    {
        Task<ISagaStepConfiguration> ResolveAsync(ISagaEvent sagaEvent, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>> versions);
    }

    internal sealed class SagaVersionStepResolver : ISagaVersionStepResolver
    {
        private readonly ISagaEventStore _sagaEventStore;
        private readonly ISagaVersionResolver _sagaVersionResolver;
        private readonly ISagaStepResolver _sagaStepResolver;

        public SagaVersionStepResolver(
            ISagaEventStore sagaEventStore,
            ISagaVersionResolver sagaVersionResolver,
            ISagaStepResolver sagaStepResolver)
        {
            _sagaEventStore = sagaEventStore;
            _sagaVersionResolver = sagaVersionResolver;
            _sagaStepResolver = sagaStepResolver;
        }

        public async Task<ISagaStepConfiguration> ResolveAsync(ISagaEvent sagaEvent, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>> versions)
        {
            var references = await _sagaEventStore
                .GetReferencesAsync(sagaEvent.ProcessId)
                .ConfigureAwait(false);

            var resolverFunc = GetVersionFunc(_sagaVersionResolver, references).Compose(GetStepFunc(_sagaStepResolver, sagaEvent, references));

            return resolverFunc(versions);
        }

        private static readonly Func<ISagaVersionResolver, IList<ISagaReference>?, Func<IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>>> GetVersionFunc =
            (sagaVersionResolver, references) => (versions) => sagaVersionResolver.Create()(references, versions);

        private static readonly Func<ISagaStepResolver, ISagaEvent, IList<ISagaReference>?, Func<IList<ISagaStepConfiguration>, ISagaStepConfiguration>> GetStepFunc =
            (sagaStepResolver, sagaEvent, references) => (steps) => sagaStepResolver.Create()(sagaEvent, references, steps);
    }
}