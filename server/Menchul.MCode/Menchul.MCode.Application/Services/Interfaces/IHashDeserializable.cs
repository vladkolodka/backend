namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IHashDeserializable
    {
        void Deserialize(IBinaryHashReader reader);
    }
}