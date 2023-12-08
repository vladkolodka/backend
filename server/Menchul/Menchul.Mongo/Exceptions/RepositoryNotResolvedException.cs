using Menchul.Application.Exceptions;

namespace Menchul.Mongo.Exceptions
{
    public class RepositoryNotResolvedException<TRepository> : AppException
    {
        public RepositoryNotResolvedException() : base($"A repository [{typeof(TRepository).Name}] can't be resolved.")
        {
        }
    }
}