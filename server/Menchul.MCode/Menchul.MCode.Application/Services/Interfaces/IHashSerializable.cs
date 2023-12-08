using Menchul.MCode.Application.Services.Interfaces;

namespace Menchul.MCode.Application.Hash
{
    public interface IHashSerializable
    {
        public void Serialize(IBinaryHashWriter writer);
    }
}