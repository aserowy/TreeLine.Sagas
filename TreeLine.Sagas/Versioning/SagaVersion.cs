using Version = SemVer.Version;

namespace TreeLine.Sagas.Versioning
{
    public interface ISagaVersion : System.IComparable<ISagaVersion>
    {
        int Major { get; }
        int Minor { get; }
        int Patch { get; }
    }

    internal sealed class SagaVersion : ISagaVersion
    {
        public SagaVersion(string version)
        {
            if (version == null)
            {
                throw new System.ArgumentNullException(nameof(version));
            }

            Version = new Version(version);
        }

        public int Major => Version.Major;
        public int Minor => Version.Minor;
        public int Patch => Version.Patch;

        internal Version Version { get; }

        public int CompareTo(ISagaVersion other)
        {
            if (!(other is SagaVersion converted))
            {
                return Version.CompareTo(new Version(other.Major, other.Minor, other.Patch));
            }

            return Version.CompareTo(converted.Version);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SagaVersion converted))
            {
                return false;
            }

            return Version.Equals(converted.Version);
        }

        public override int GetHashCode() => Version.GetHashCode();

        public override string ToString() => Version.ToString();
    }
}