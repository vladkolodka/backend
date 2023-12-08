using System;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IBinaryHashReader
    {
        DateTime NextDateTime();
        byte[] NextBytes();
    }
}