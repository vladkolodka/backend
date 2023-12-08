using Menchul.Core.Types;
using System;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IIdProvider
    {
        Guid NextGuid();
        IBsonId NextCrypto12ByteId();
        IBsonId Next12ByteId();
    }
}