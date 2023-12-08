using Menchul.Base.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace Menchul.Base.Extensions
{
    public static class AppDomainExtensions
    {
        private static readonly AssemblyComparer AssemblyComparer = new();

        public static AssemblyName[] LoadAssemblies(this AppDomain domain)
        {
            Assembly[] domainAssemblies = domain.GetAssemblies();
            AssemblyName[] assemblies = domainAssemblies.Select(x => x.GetName())
                .Union(domainAssemblies.SelectMany(x => x.GetReferencedAssemblies()))
                .Distinct(AssemblyComparer)
                .OrderBy(s => s.FullName)
                .ThenBy(s => s.Version)
                .ToArray();

            return assemblies;
        }
    }
}