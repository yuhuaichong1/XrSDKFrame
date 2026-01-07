using Newtonsoft.Json;
using System;
using UnityEngine;
namespace XrCode
{
    public class Hash128Converter : JsonConverter<Hash128>
    {
        public override void WriteJson(JsonWriter writer, Hash128 value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override Hash128 ReadJson(JsonReader reader, Type objectType, Hash128 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string hashString = (string)reader.Value;
            return Hash128.Parse(hashString);
        }
    }
}
