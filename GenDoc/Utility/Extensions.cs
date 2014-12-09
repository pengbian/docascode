using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using DocAsCode.EntityModel;
using Newtonsoft.Json.Linq;

namespace DocAsCode.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileExtensions
    {
        private static char[] InvalidFilePathChars = Path.GetInvalidFileNameChars();
        public static string ToValidFilePath(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Path.GetRandomFileName();
            }

            return new string(input.Select(s => InvalidFilePathChars.Contains(s) ? '_' : s).ToArray());
        }
    }

    public static class DocMetadataExtensions
    {
        public static void WriteHtml()
        {

        }

        public static void WriteMetadata(this DocMetadata metadata, TextWriter writer)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            JsonSerializer serializer = new JsonSerializer();
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;
            serializer.Converters.Add(new IdentityJsonConverter());
            serializer.Serialize(writer, metadata);
        }

        public static T LoadMetadata<T>(TextReader reader) where T : DocMetadata
        {
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), new IdentityJsonConverter());
        }

        private class IdentityJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                if (objectType == typeof(Identity))
                {
                    return true;
                }

                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {

                if (reader.TokenType != JsonToken.String)
                {
                    return null;
                }

                return new Identity((string)reader.Value);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
