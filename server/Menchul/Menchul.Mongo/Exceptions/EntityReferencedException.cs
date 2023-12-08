using Menchul.Application.Exceptions;
using System.Collections.Generic;

namespace Menchul.Mongo.Exceptions
{
    public class EntityReferencedException : AppException
    {
        public EntityReferencedException() : base($"The entity is referenced and can't be deleted.")
        {
        }

        public EntityReferencedException(Dictionary<string, IEnumerable<string>> references) : this()
        {
            References = references;
        }

        public IReadOnlyDictionary<string, IEnumerable<string>> References { get; }
    }
}