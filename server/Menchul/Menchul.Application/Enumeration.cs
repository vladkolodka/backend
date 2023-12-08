using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Menchul.Application
{
    // TODO move to Menchul.Core
    public abstract class Enumeration : IComparable, IEquatable<Enumeration>
    {
        public string Name { get; }

        public int Id { get; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        public bool Equals(Enumeration other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Enumeration) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}