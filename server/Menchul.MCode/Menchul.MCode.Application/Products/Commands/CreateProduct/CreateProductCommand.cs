using Convey.CQRS.Commands;

namespace Menchul.MCode.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand : ProductCommandBase, ICommand
    {
        public string ParentProductId { get; set; }

        public long EAN { get; set; }
    }
}