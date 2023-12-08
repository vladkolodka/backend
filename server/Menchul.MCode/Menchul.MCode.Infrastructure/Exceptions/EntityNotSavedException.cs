using System;

namespace Menchul.MCode.Infrastructure.Exceptions
{
    public class EntityNotSavedException : ApplicationException
    {
        public EntityNotSavedException() : base(
            "The entity wasn't saved due to the internal server error. Try requesting again.")
        {
        }
    }
}