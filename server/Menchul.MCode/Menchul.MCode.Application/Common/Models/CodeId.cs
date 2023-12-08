using Menchul.MCode.Application.Hash;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using System;

namespace Menchul.MCode.Application.Common.Models
{
    /// <summary>
    /// Hash DTO
    /// </summary>
    public class CodeId : IHashSerializable, IHashDeserializable
    {
        public const int IdSize = Id12Bytes.Size;

        public byte[] Id { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] ParentId { get; set; }

        public void Serialize(IBinaryHashWriter writer)
        {
            writer.NextBytes(Id);
            writer.NextDateTime(DateTime);
            writer.NextBytes(ParentId);
        }

        public void Deserialize(IBinaryHashReader reader)
        {
            Id = reader.NextBytes();
            DateTime = reader.NextDateTime();
            ParentId = reader.NextBytes();
        }
    }
}