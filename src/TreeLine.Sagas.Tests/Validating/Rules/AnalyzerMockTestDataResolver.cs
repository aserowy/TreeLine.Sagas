using System.Collections.Generic;
using TreeLine.Sagas.Tests.Mocks;
using TreeLine.Sagas.Validating.Analyzing;

namespace TreeLine.Sagas.Tests.Validating.Rules
{
    internal sealed class AnalyzerMockTestDataResolver
    {
        public IDictionary<AnalyzerMockTestDataType, IList<ISagaProfileAnalyzer>> Get()
        {
            return new Dictionary<AnalyzerMockTestDataType, IList<ISagaProfileAnalyzer>>
            {
                {AnalyzerMockTestDataType.NoSagasRegistered,  GetNoSagasRegistered()},
                {AnalyzerMockTestDataType.ValidAnalyzers,  GetValidAnalyzers()},
                {AnalyzerMockTestDataType.EqualAnalyzerIdentifier,  GetEqualAnalyzerIdentifier()},
                {AnalyzerMockTestDataType.EqualVersionIdentifier,  GetEqualVersionIdentifier()},
                {AnalyzerMockTestDataType.VersionWithoutSteps,  GetVersionWithoutSteps()}
            };
        }
        private static IList<ISagaProfileAnalyzer> GetNoSagasRegistered()
        {
            return new List<ISagaProfileAnalyzer>();
        }

        private static IList<ISagaProfileAnalyzer> GetValidAnalyzers()
        {
            var result = new List<ISagaProfileAnalyzer>();

            var analyzer01 = new SagaProfileAnalyzerMock("1");
            result.Add(analyzer01);

            analyzer01
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer01
                .AddVersion("1.2.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            var analyzer02 = new SagaProfileAnalyzerMock("2");
            result.Add(analyzer02);

            analyzer02
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer02
                .AddVersion("1.2.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();


            return result;
        }

        private static IList<ISagaProfileAnalyzer> GetEqualAnalyzerIdentifier()
        {
            var result = new List<ISagaProfileAnalyzer>();

            var analyzer01 = new SagaProfileAnalyzerMock("1");
            result.Add(analyzer01);

            analyzer01
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer01
                .AddVersion("1.2.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            var analyzer02 = new SagaProfileAnalyzerMock("1");
            result.Add(analyzer02);

            analyzer02
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer02
                .AddVersion("1.2.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();


            return result;
        }

        private static IList<ISagaProfileAnalyzer> GetEqualVersionIdentifier()
        {
            var result = new List<ISagaProfileAnalyzer>();

            var analyzer01 = new SagaProfileAnalyzerMock("1");
            result.Add(analyzer01);

            analyzer01
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer01
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            var analyzer02 = new SagaProfileAnalyzerMock("2");
            result.Add(analyzer02);

            analyzer02
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer02
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();


            return result;
        }

        private static IList<ISagaProfileAnalyzer> GetVersionWithoutSteps()
        {
            var result = new List<ISagaProfileAnalyzer>();

            var analyzer01 = new SagaProfileAnalyzerMock("1");
            result.Add(analyzer01);

            analyzer01.AddVersion("1.1.0");
            analyzer01.AddVersion("1.2.0");

            var analyzer02 = new SagaProfileAnalyzerMock("2");
            result.Add(analyzer02);

            analyzer02
                .AddVersion("1.1.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();

            analyzer02
                .AddVersion("1.2.0")
                .AddStep<SagaEvent01, SagaStep01Mock>()
                .AddStep<SagaEvent02, SagaStep03Mock>();


            return result;
        }
    }
}
