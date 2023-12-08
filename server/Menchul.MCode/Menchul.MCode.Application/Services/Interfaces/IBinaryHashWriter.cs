using System;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IBinaryHashWriter
    {
        void NextDateTime(DateTime dateTime);
        void NextBytes(byte[] bytes);

        byte[] GetResult();
    }
}