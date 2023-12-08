using Menchul.Core.Types;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Infrastructure.Types;
using MongoDB.Bson;
using System;
using System.Security.Cryptography;

namespace Menchul.MCode.Infrastructure.Services
{
    public class IdProvider : IIdProvider
    {
        public Guid NextGuid()
        {
            return Guid.NewGuid();
        }

        public IBsonId NextCrypto12ByteId()
        {
            byte[] idData = new byte[CodeId.IdSize];

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(idData);
            }

            return new BsonId(new ObjectId(idData));
        }

        public IBsonId Next12ByteId()
        {
            return new BsonId(ObjectId.GenerateNewId());
        }
    }
}