using System;

namespace TreeLine.Sagas.Versioning
{
    public interface ISagaVersion : IComparable<ISagaVersion>
    {
        int Major { get; }
        int Minor { get; }
        int Patch { get; }
    }
}