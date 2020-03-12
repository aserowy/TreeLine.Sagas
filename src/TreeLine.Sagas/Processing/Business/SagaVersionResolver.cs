using System;
using System.Collections.Generic;
using System.Linq;
using TreeLine.Sagas.Building;
using TreeLine.Sagas.ReferenceStore;
using TreeLine.Sagas.Versioning;

namespace TreeLine.Sagas.Processing.Business
{
    internal interface ISagaVersionResolver
    {
        Func<IList<ISagaReference>?, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> Create();
    }

    internal sealed class SagaVersionResolver : ISagaVersionResolver
    {
        public Func<IList<ISagaReference>?, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> Create()
        {
            return _resolve;
        }

        private static readonly Func<IList<ISagaReference>?, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> _resolve =
            (references, versions) =>
            {
                if (versions.Count.Equals(0))
                {
                    throw new InvalidOperationException("No version configured.");
                }

                if (references?.Count.Equals(0) != false)
                {
                    return _getCurrentVersionFunc(versions);
                }

                var referenceId = _getReferenceIdFunc(references);
                var result = _getReferenceVersionFunc(references, versions);

                return result ?? throw new ArgumentOutOfRangeException($"No version found for reference id {referenceId}");
            };

        private static readonly Func<IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> _getCurrentVersionFunc =
            versions =>
            {
                var result = _getHighestVersionFunc(versions.Keys);

                return versions[result];
            };

        private static readonly Func<IList<ISagaReference>, IDictionary<ISagaVersion, IList<ISagaStepConfiguration>>, IList<ISagaStepConfiguration>> _getReferenceVersionFunc =
            (references, versions) =>
            {
                var referenceVersion = _getHighestVersionFunc(references.Select(rfrnc => rfrnc.Version));
                var possibleVersions = versions
                    .Keys
                    .Where(vrsn => vrsn.Major.Equals(referenceVersion.Major) && vrsn.Minor.Equals(referenceVersion.Minor));

                var version = _getHighestVersionFunc(possibleVersions);

                return versions[version];
            };

        private static readonly Func<IEnumerable<ISagaVersion>, ISagaVersion> _getHighestVersionFunc =
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

        private static readonly Func<IList<ISagaReference>, Guid> _getReferenceIdFunc =
            references =>
            {
                return references
                    .Select(rfrnc => rfrnc.ReferenceId)
                    .Distinct()
                    .Single();
            };
    }
}