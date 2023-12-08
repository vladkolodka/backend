using Menchul.Core.Events;
using Menchul.MCode.Core.Events;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Packages.Events.Domain.Handlers
{
    public class PackageCreatedEventHandler : IDomainEventHandler<PackageCreatedEvent>
    {
        private readonly IPackageRepository _repository;
        private readonly ILogger<PackageCreatedEventHandler> _logger;

        public PackageCreatedEventHandler(IPackageRepository repository, ILogger<PackageCreatedEventHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(PackageCreatedEvent @event)
        {
            _logger.LogInformation("Going to search for package that conflicts with package {PackageId}",
                @event.Package.Id.Value);

            var conflictingPackages = await _repository.GetConflictingAsync(@event.Package);

            if (conflictingPackages?.Any() != true)
            {
                _logger.LogInformation("Conflicts not found for package {PackageId}", @event.Package.Id.Value);
                return;
            }

            _logger.LogInformation("Going to invalidate conflicting packages for package {PackageId}: {Conflicts}",
                @event.Package.Id.Value, conflictingPackages.Select(p => p.Id.Value));

            conflictingPackages.ForEach(p => p.Invalidate());

            await _repository.UpdateStateAllAsync(conflictingPackages);

            _logger.LogInformation("Invalidated conflicting packages for package {PackageId}", @event.Package.Id.Value);
        }
    }
}