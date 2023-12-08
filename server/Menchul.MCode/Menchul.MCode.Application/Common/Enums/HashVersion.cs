using Menchul.Application;
using Menchul.MCode.Application.Services.Hash;

namespace Menchul.MCode.Application.Common.Enums
{
    public class HashVersion : Enumeration
    {
        private HashVersion(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// implementation hidden
        /// </summary>
        public static readonly HashVersion V1 = new HashVersion(1, nameof(V1));


        /// <summary>
        /// implementation hidden
        /// </summary>
        public static readonly HashVersion V2 = new HashVersion(1, nameof(V2));
    }
}