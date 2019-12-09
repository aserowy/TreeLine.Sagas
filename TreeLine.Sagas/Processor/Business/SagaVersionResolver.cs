using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Builder;
using TreeLine.Sagas.EventStore;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Processor.Business
{
    internal interface ISagaVersionResolver
    {
        Func<IList<ISagaReference>?, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> Create();
    }

    internal sealed class SagaVersionResolver : ISagaVersionResolver
    {
        public Func<IList<ISagaReference>?, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> Create()
        {
            return Resolve;
        }

        private static readonly Func<IList<ISagaReference>?, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> Resolve =
            (references, versions) =>
            {
                if (references is null || references.Count == 0)
                {
                    return GetCurrentVersionFunc(versions);
                }

                var referenceId = GetReferenceIdFunc(references);
                var result = GetReferenceVersionFunc(references, versions);

                return result ?? throw new ArgumentOutOfRangeException($"No version found for reference id {referenceId}");
            };

        private static readonly Func<IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> GetCurrentVersionFunc =
            versions =>
            {
                if (versions.Count.Equals(0))
                {
                    throw new InvalidOperationException("No version configured.");
                }

                var result = GetHighestVersionFunc(versions.Keys);

                return versions[result];
            };

        private static readonly Func<IList<ISagaReference>, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> GetReferenceVersionFunc =
            (references, versions) =>
            {
                var referenceVersion = GetHighestVersionFunc(references.Select(rfrnc => rfrnc.Version));
                var possibleVersions = versions
                    .Keys
                    .Where(vrsn => vrsn.Major.Equals(referenceVersion.Major) && vrsn.Minor.Equals(referenceVersion.Minor));

                var version = GetHighestVersionFunc(possibleVersions);

                return versions[version];
            };

        private static readonly Func<IEnumerable<ISagaVersion>, ISagaVersion> GetHighestVersionFunc =
            versions =>
            {
                ISagaVersion? result = null;
                foreach (var version in versions)
                {
                    if (result is null)
                    {
                        result = version;
                        continue;
                    }

                    if (version.CompareTo(result) > 0)
                    {
                        result = version;
                    }
                }

                if (result is null)
                {
                    throw new InvalidOperationException("No version resolved.");
                }

                return result;
            };

        private static readonly Func<IList<ISagaReference>, Guid> GetReferenceIdFunc =
            references =>
            {
                return references
                    .Select(rfrnc => rfrnc.ReferenceId)
                    .Distinct()
                    .Single();
            };
    }
}