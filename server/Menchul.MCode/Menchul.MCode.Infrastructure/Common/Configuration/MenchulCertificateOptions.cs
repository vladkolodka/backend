namespace Menchul.MCode.Infrastructure.Common.Configuration
{
    public class MenchulCertificateOptions
    {
        public static string SectionName = "certificate";

        public string PfxFile { get; set; }
        public string PasswordFile { get; set; }
    }
}