using Menchul.Application;

namespace Menchul.MCode.Application.Common.Enums
{
    public class QrCodeType : Enumeration
    {
        public string Path { get; }
        public HashVersion HashVersion { get; }

        private QrCodeType(int id, string name, string path, HashVersion hashVersion) : base(id, name)
        {
            Path = path;
            HashVersion = hashVersion;
        }

        /// <summary>
        /// New codes shouldn't be generated using this code type.
        /// </summary>
        public static readonly QrCodeType ProductUnitV1 = new(1, nameof(ProductUnitV1), "hidden1", HashVersion.V1);
        public static readonly QrCodeType ProductUnitV2 = new(2, nameof(ProductUnitV2), "hidden2", HashVersion.V2);
        public static readonly QrCodeType PackageV1 = new(3, nameof(PackageV1), "hidden3", HashVersion.V2);
    }
}