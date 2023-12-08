using Convey.CQRS.Commands;

namespace Menchul.MCode.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : ICommand
    {
        public string Id { get; set; }
    }
}