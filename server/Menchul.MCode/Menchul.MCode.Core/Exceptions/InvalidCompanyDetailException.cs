namespace Menchul.MCode.Core.Exceptions
{
    public class InvalidCompanyDetailException : InvalidDetailException
    {
        public InvalidCompanyDetailException(string detailName, object value = null)
            : base("company", detailName, value)
        {
        }
    }
}