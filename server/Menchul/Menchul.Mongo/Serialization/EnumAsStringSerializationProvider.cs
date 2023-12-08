﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace Menchul.Mongo.Serialization
{
    // https://jira.mongodb.org/browse/CSHARP-1728
    public class EnumAsStringSerializationProvider : BsonSerializationProviderBase
    {
        public override IBsonSerializer GetSerializer(Type type, IBsonSerializerRegistry serializerRegistry)
        {
            if (!type.IsEnum)
            {
                return null;
            }

            var enumSerializerType = typeof(EnumSerializer<>).MakeGenericType(type);
            var enumSerializerConstructor = enumSerializerType.GetConstructor(new[] {typeof(BsonType)});
            var enumSerializer = (IBsonSerializer) enumSerializerConstructor!.Invoke(new object[] {BsonType.String});

            return enumSerializer;
        }
    }
}