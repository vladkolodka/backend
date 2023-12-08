using System;

namespace Menchul.MCode.Core.Exceptions
{
    /// <summary>
    /// This exception should never be thrown as the client company id is provided on the signed token.
    /// </summary>
    public class ClientCompanyNotFoundException : NotFoundException
    {
        public ClientCompanyNotFoundException(Guid id) : base(
            $"Houston, we have a problem... The client company [{id}] on whose behalf you are performing the transaction was not found, try to request a new access token.")
        {
        }
        public ClientCompanyNotFoundException() : base(
            $"Houston, we have a problem... The client company on whose behalf you are performing the transaction was not found, try to request a new access token.")
        {
        }
    }
}