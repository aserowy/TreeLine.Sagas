using System;
using System.Diagnostics.CodeAnalysis;

namespace TreeLine.Messaging.Typing
{
    [SuppressMessage("Design", "CA1036:Override methods on comparable types",
        Justification = "Since Enumeration is abstract, different concrete implementations must not be compareable.")]
    public abstract class EnumerationBase : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected EnumerationBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object other)
        {
            if (!(other is EnumerationBase otherValue))
            {
                throw new InvalidCastException($"{nameof(other)} is not of type {nameof(EnumerationBase)}.");
            }

            if (GetType().Equals(otherValue.GetType()))
            {
                throw new InvalidCastException($"{nameof(otherValue)} is not of type {GetType().Name}.");
            }

            return Id.CompareTo(otherValue.Id);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EnumerationBase otherValue))
            {
                return false;
            }

            var typeMatches = GetType().Equals(otherValue.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
