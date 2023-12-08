namespace Menchul.MCode.Application.Common.Dto
{
    public record QrResultDto
    {
        public string Id { get; set; }

        public string ImageBase64 { get; set; }
    }
}