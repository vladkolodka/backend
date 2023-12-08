using System;

namespace Menchul.Mongo.Exceptions
{
    public class PermanentRelationDeleteException : ApplicationException
    {
        public PermanentRelationDeleteException() : base(
            $"The entity can't be deleted because it has permanent reference(s).")
        {
        }
    }
}