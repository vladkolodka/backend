namespace Menchul.Core.Types
{
    // TODO add startup inheritance check
    public interface IBsonId
    {
        byte[] ToByteArray();
        string ToString();
    }
}