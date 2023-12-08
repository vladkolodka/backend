namespace Menchul.MCode.Application.Common.Configuration
{
    public class QrGenerationOptions
    {
        public static string SectionName = "qrGeneration";

        public string BaseUrl { get; set; }
        public string QueryParameterName { get; set; }
        public string CertificateThumbprint { get; set; }
    }
}