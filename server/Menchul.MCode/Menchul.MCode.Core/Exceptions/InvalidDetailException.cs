using Menchul.Core.Exceptions;

namespace Menchul.MCode.Core.Exceptions
{
    public abstract class InvalidDetailException : DomainException
    {
        public string Name { get; }

        protected InvalidDetailException(string entity, string detailName, object value = null)
            : base(value == null
                ? $"The value is not valid for {entity}' {detailName}."
                : $"The value [{value}] is not valid for {entity}' {detailName}.")
        {
            Name = detailName;
        }
    }
}