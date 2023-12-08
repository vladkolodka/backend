using Convey.Logging.Options;

namespace Menchul.MCode.Api
{
    public class FileOptionsAdvanced : FileOptions
    {
        public const string ConfigPath = "logger:fileAdvanced";

        public string Format { get; set; }
        public bool Structured { get; set; }
    }
}