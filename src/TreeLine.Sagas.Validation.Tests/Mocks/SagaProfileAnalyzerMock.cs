using System;
using System.Collections.Generic;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.Processing;
using TreeLine.Sagas.Validation.Analyzing;

namespace TreeLine.Sagas.Validation.Tests.Mocks
{
    internal sealed class SagaProfileAnalyzerMock : ISagaProfileAnalyzer
    {
        public SagaProfileAnalyzerMock(string profileName)
        {
            ProfileName = profileName;

            VersionAnalyzer = new List<ISagaProfileVersionAnalyzer>();
        }

        public IList<ISagaProfileVersionAnalyzer> VersionAnalyzer { get; }
        public string ProfileName { get; private set; }

        public ISagaVersionBuilder AddVersion(string version)
        {
            var versionAnalyzer = new SagaProfileVersionAnalyzerMock(version);
            VersionAnalyzer.Add(versionAnalyzer);

            return versionAnalyzer;
        }

        public ISagaProcessor Build()
        {
            throw new MethodAccessException("Method must not get called to analyze profile structure.");
        }
    }
}
