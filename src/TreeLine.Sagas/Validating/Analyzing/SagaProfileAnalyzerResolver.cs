using System.Collections.Generic;

namespace TreeLine.Sagas.Validating.Analyzing
{
    internal interface ISagaProfileAnalyzerResolver
    {
        IEnumerable<ISagaProfileAnalyzer> Get();
    }

    internal sealed class SagaProfileAnalyzerResolver : ISagaProfileAnalyzerResolver
    {
        private readonly ISagaProfileAnalyzerFactory _factory;
        private readonly IEnumerable<ISagaProfile> _profiles;

        public SagaProfileAnalyzerResolver(
            ISagaProfileAnalyzerFactory factory,
            IEnumerable<ISagaProfile> profiles)
        {
            _factory = factory;
            _profiles = profiles;
        }

        public IEnumerable<ISagaProfileAnalyzer> Get()
        {
            var result = new List<ISagaProfileAnalyzer>();
            foreach (var profile in _profiles)
            {
                var analyzer = _factory.Create(profile.GetType().Name);
                profile.Configure(analyzer);

                result.Add(analyzer);
            }

            return result;
        }
    }
}
