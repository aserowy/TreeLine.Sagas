using System;
using System.Collections.Generic;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Processing;

namespace TreeLine.Sagas.Validation.Analyzing
{
    internal interface ISagaProfileAnalyzer : ISagaProcessorBuilder
    {
        IList<ISagaProfileVersionAnalyzer> VersionAnalyzer { get; }
        string ProfileName { get; }
    }

    internal sealed class SagaProfileAnalyzer : ISagaProfileAnalyzer
    {
        private readonly ISagaProfileVersionAnalyzerFactory _factory;

        public SagaProfileAnalyzer(string profileName, ISagaProfileVersionAnalyzerFactory factory)
        {
            ProfileName = profileName;

            _factory = factory;

            VersionAnalyzer = new List<ISagaProfileVersionAnalyzer>();
        }

        public IList<ISagaProfileVersionAnalyzer> VersionAnalyzer { get; }
        public string ProfileName { get; }

        public ISagaVersionBuilder AddVersion(string version)
        {
            var versionAnalyzer = _factory.Create(version);
            VersionAnalyzer.Add(versionAnalyzer);

            return versionAnalyzer;
        }

        public ISagaProcessor Build()
        {
            throw new MethodAccessException("Method must not get called to analyze profile structure.");
        }
    }
}
