using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Queries;

namespace Menchul.MCode.Application.ProductUnits.Queries
{
    public class GetProductUnitByHashQuery : QueryBase, IQuery<PassportDto>
    {
        public long EAN { get; set; }
        public string QR { get; set; }
        public QrCodeType CodeType { get; set; }
    }
}