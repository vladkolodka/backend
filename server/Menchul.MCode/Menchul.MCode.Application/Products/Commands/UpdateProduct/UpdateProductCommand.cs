using Convey.CQRS.Commands;

namespace Menchul.MCode.Application.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand : ProductCommandBase, ICommand
    {
        public string Id { get; set; }
    }
}