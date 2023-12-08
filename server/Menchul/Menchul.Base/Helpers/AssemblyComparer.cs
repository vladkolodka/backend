using System.Collections.Generic;
using System.Reflection;

namespace Menchul.Base.Helpers
{
    internal class AssemblyComparer : IEqualityComparer<AssemblyName>
    {
        public bool Equals(AssemblyName x, AssemblyName y)
        {
            return string.Equals(x?.FullName, y?.FullName);
        }

        public int GetHashCode(AssemblyName obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}