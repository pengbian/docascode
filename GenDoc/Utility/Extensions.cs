using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using EntityModel;
using Newtonsoft.Json.Converters;

/// <summary>
/// The utility class for docascode project
/// </summary>
namespace DocAsCode.Utility
{
    public static class StringExtension
    {
        public static string[] ToArray(this string input, StringSplitOptions option, params char[] delimiter)
        {
            if (input == null)
            {
                return null;
            }

            return input.Split(delimiter, option);
        }

        public static string ToDelimitedString(this IEnumerable<string> input, string delimiter = ",")
        {
            if (input == null)
            {
                return null;
            }

            return string.Join(delimiter, input);
        }
    }

    /// <summary>
    /// The converter to transform strings delimited by comma into string arrays
    /// </summary>
    public class DelimitedStringArrayConverter : TypeConverter
    {
        private readonly char[] _delimiter = { ',', };

        /// <summary>
        /// Specifies if the current type can be converted from
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            return stringValue.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            throw new NotImplementedException();
        }
    }

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

    public static class JsonUtility
    {
        public static void Serialize<T>(T input, TextWriter writer)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            JsonSerializer serializer = new JsonSerializer();
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;
            serializer.Converters.Add(new CustomizedJsonConverters.IdentityJsonConverter());
            serializer.Converters.Add(new StringEnumConverter());
            serializer.Serialize(writer, input);
        }
    }

    //public static class DocMetadataExtensions
    //{
    //    public static void WriteHtml()
    //    {

    //    }

    //    public static void WriteMetadata(this EntityModel.DocMetadata metadata, TextWriter writer)
    //    {
    //        JsonSerializerSettings settings = new JsonSerializerSettings();
    //        settings.Formatting = Formatting.Indented;
    //        settings.DefaultValueHandling = DefaultValueHandling.Ignore;
    //        JsonSerializer serializer = new JsonSerializer();
    //        serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
    //        serializer.Formatting = Formatting.Indented;
    //        serializer.Converters.Add(new IdentityJsonConverter());
    //        serializer.Serialize(writer, metadata);
    //    }

    //    public static T LoadMetadata<T>(TextReader reader) where T : EntityModel.DocMetadata
    //    {
    //        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), new IdentityJsonConverter());
    //    }

    //    private class IdentityJsonConverter : JsonConverter
    //    {
    //        public override bool CanConvert(Type objectType)
    //        {
    //            if (objectType == typeof(Identity))
    //            {
    //                return true;
    //            }

    //            return false;
    //        }

    //        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //        {

    //            if (reader.TokenType != JsonToken.String)
    //            {
    //                return null;
    //            }

    //            return new Identity((string)reader.Value);
    //        }

    //        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //        {
    //            writer.WriteValue(value.ToString());
    //        }
    //    }
    //}

    public static class CustomizedJsonConverters
    {
        public class IdentityJsonConverter : JsonConverter
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
