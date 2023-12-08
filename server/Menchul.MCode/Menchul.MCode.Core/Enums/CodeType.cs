namespace Menchul.MCode.Core.Enums
{
    public enum CodeType
    {
        /// <summary> Producer article number </summary>
        /// <remarks> ASCII Latin-1 characters defined in ISO/IEC 8859-1. 128 bytes long. </remarks>
        ArticleNumber,

        /// <summary> https://en.wikipedia.org/wiki/International_Article_Number </summary>
        /// <remarks> ASCII Latin-1 characters defined in ISO/IEC 8859-1. 128 bytes long. </remarks>
        EAN2,

        ///  https://en.wikipedia.org/wiki/EAN-5
        EAN5,

        /// https://en.wikipedia.org/wiki/EAN-8
        EAN8,

        /// https://en.wikipedia.org/wiki/International_Article_Number
        EAN13,

        EAN14,

        /// <summary> International Standard Book Number </summary>
        /// https://en.wikipedia.org/wiki/International_Standard_Book_Number
        ISBN,

        SerialNumber,
        VIN,
        VehicleRegistrationNumber
    }
}