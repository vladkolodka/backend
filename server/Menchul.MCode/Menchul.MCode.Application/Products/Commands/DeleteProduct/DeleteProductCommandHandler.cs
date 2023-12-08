using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Policies;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IClientCompanyOwnerPolicy _ownerPolicy;
        private readonly IAggregateIdParser _idParser;
        private readonly ILogger<DeleteProductCommandHandler> _logger;
        private readonly Guid _clientCompanyId;

        public DeleteProductCommandHandler(IProductRepository repository, IClientCompanyOwnerPolicy ownerPolicy,
            IAggregateIdParser idParser, IAppContext appContext, ILogger<DeleteProductCommandHandler> logger)
        {
            _repository = repository;
            _ownerPolicy = ownerPolicy;
            _idParser = idParser;
            _logger = logger;

            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task HandleAsync(DeleteProductCommand command)
        {
            _logger.LogInformation("Trying to delete product {ProductId}", command.Id);

            var productId = _idParser.Id12BytesFromString(command.Id);
            var product = await _repository.GetAsync(productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            if (!_ownerPolicy.IsOwnedBy(product, _clientCompanyId))
            {
                throw new ProductNotOwnedException(command.Id);
            }

            await _repository.DeleteAsync(productId);

            _logger.LogInformation("Deleted product {ProductId}", command.Id);
        }
    }
}