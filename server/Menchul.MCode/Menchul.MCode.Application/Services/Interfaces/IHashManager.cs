using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Hash;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IHashManager
    {
        // byte[] BuildCodeHash(CodeId codeId);
        // byte[] BuildSignedCodeHash(byte[] dataHash);
        // byte[] BuildSignedCodeHash(CodeId codeId);
        byte[] BuildSignedCodeHash<T>(T hashModel, HashVersion hashVersion) where T : IHashSerializable;

        // CodeId DecodeCodeHash(byte[] dataHash);
        // CodeId DecodeSignedCodeHash(byte[] hash);

        T DecodeSignedCodeHash<T>(byte[] hash, HashVersion hashVersion) where T : IHashDeserializable, new();
    }
}