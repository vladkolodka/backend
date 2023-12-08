using Menchul.Core.Exceptions;
using System;

namespace Menchul.MCode.Core.Exceptions
{
    public class InvalidId12BitException : DomainException
    {
        public string Id { get; }

        public InvalidId12BitException(string id) : base($"Invalid aggregate id: {id}")
            => Id = id;

        public InvalidId12BitException(byte[] id) : base("0x" + BitConverter.ToString(id).Replace("-", ""))
        {
        }
    }
}