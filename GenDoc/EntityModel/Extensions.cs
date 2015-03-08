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
using System.Collections;
using YamlDotNet.Serialization;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

/// <summary>
/// The utility class for docascode project
/// </summary>
namespace DocAsCode.Utility
{
    public static class TypeExtension
    {
        public static bool IsA(this Type thisType, Type targetType)
        {
            return targetType.IsAssignableFrom(thisType);
        }

        public static string ToId(this Identity id)
        {
            if (id == null)
            {
                return null;
            }

            string str = id.ToString();
            if (str.StartsWith("{") || str.Length < 3)
            {
                return str;
            }

            return id.ToString().Substring(2);
        }
    }
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

        public static void SaveFileListToFile(List<string> fileList, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || fileList == null || fileList.Count == 0)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                fileList.ForEach(s => writer.WriteLine(s));
            }
        }

        public const string ListFileExtension = ".list";

        public static List<string> GetFileListFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }
            List<string> fileList = new List<string>();

            if (Path.GetExtension(filePath) == ListFileExtension)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        fileList.Add(reader.ReadLine());
                    }
                }
            }
            else
            {
                fileList = filePath.ToArray(StringSplitOptions.RemoveEmptyEntries, ',').ToList();
            }

            return fileList;
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="basePath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="absolutePath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string MakeRelativePath(string basePath, string absolutePath)
        {
            if (string.IsNullOrEmpty(basePath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(absolutePath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(Path.GetFullPath(basePath));
            Uri toUri = new Uri(Path.GetFullPath(absolutePath));

            if (fromUri.Scheme != toUri.Scheme) { return absolutePath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        public static string FormatPath(this string path, UriKind kind, string basePath = null)
        {
            if (kind == UriKind.RelativeOrAbsolute)
            {
                return path;
            }
            if (kind == UriKind.Absolute)
            {
                return Path.GetFullPath(path);
            }
            if (kind == UriKind.Relative)
            {
                if (string.IsNullOrEmpty(basePath))
                {
                    return path;
                }

                return MakeRelativePath(basePath, path);
            }

            return null;
        }
    }

    public static class YamlUtility
    {
        public static Serializer Serializer = new Serializer();
        public static Deserializer Deserializer = new Deserializer();
    }

    public static class JsonUtility
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };
        private static JsonSerializer _serializer;

        private static JsonSerializer _yamlserializer;
        static JsonUtility()
        {
            _serializer = new JsonSerializer();
            _serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            _serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            _serializer.Converters.Add(new CustomizedJsonConverters.IdentityMappingConverter<NamespaceMetadata>());
            _serializer.Converters.Add(new CustomizedJsonConverters.IdentityJsonConverter());
            _serializer.Converters.Add(new CustomizedJsonConverters.BaseMetadataInheritClassJsonConverter());
            _serializer.Converters.Add(new CustomizedJsonConverters.SyntaxDescriptionInheritClassJsonConverter());
            _serializer.Converters.Add(new StringEnumConverter());

            _yamlserializer = JsonSerializer.CreateDefault();
        }
        public static void Serialize<T>(T input, TextWriter writer)
        {
            _serializer.Serialize(writer, input);
        }

        public static T Deserialize<T>(TextReader reader)
        {
            using (JsonReader jsonReader = new JsonTextReader(reader))
            {
               return _serializer.Deserialize<T>(jsonReader);
            }
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

    public static class CustomizedYamlConverters
    {
        public class DefaultYamlConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                if (objectType == typeof(object))
                {
                    return true;
                }

                return false;
            }

            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        public class IdentityYamlConverter : JsonConverter
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

        /// <summary>
        /// Dictionary keys are not regarded as values and will not be run through the JsonConverters. 
        /// http://stackoverflow.com/questions/6845364/json-net-specify-converter-for-dictionary-keys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class IdentityMappingConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                if (objectType.IsA(typeof(IdentityMapping<T>)))
                {
                    return true;
                }

                return false;
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }

                var valueType = objectType.GetGenericArguments()[0];

                // Create a intermediate dictionary with key as string
                var intermediateDictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);
                var intermediateDictionary = (IDictionary)Activator.CreateInstance(intermediateDictionaryType);
                serializer.Populate(reader, intermediateDictionary);

                var finalDictionary = (IDictionary)Activator.CreateInstance(objectType);
                foreach (DictionaryEntry pair in intermediateDictionary)
                {
                    finalDictionary.Add(new Identity(pair.Key.ToString()), pair.Value);
                }

                return finalDictionary;
            }
        }

        public class BaseMetadataInheritClassJsonConverter : InheritClassJsonConverter<IMetadata>
        {
            protected override object CreateInheritClass(JObject jsonObject)
            {
                JToken token;
                if (jsonObject.TryGetValue(MetadataConstant.MemberType, out token))
                {
                    var typeName = token.Value<string>();
                    MemberType memberType;
                    if (Enum.TryParse(typeName, true, out memberType))
                    {
                        switch (memberType)
                        {
                            case MemberType.Namespace:
                                return new NamespaceMetadata();
                            case MemberType.Class:
                            case MemberType.Delegate:
                            case MemberType.Interface:
                            case MemberType.Enum:
                            case MemberType.Struct:
                                return new NamespaceMemberMetadata();
                            case MemberType.Field:
                            case MemberType.Event:
                            case MemberType.Method:
                            case MemberType.Property:
                            case MemberType.Constructor:
                                return new NamespaceMembersMemberMetadata();
                        }
                    }
                }
                return null;
            }
        }

        public class SyntaxDescriptionInheritClassJsonConverter : InheritClassJsonConverter<ISyntaxDescription>
        {
            protected override object CreateInheritClass(JObject jsonObject)
            {
                JToken token;
                if (jsonObject.TryGetValue(MetadataConstant.SyntaxType, out token))
                {
                    var typeName = token.Value<string>();
                    SyntaxType memberType;
                    if (Enum.TryParse(typeName, true, out memberType))
                    {
                        switch (memberType)
                        {
                            case SyntaxType.ParameterSyntax:
                                return new ParameterDescription();
                            case SyntaxType.MethodSyntax:
                                return new MethodSyntaxDescription();
                            case SyntaxType.ConstructorSyntax:
                                return new ConstructorSyntaxDescription();
                            case SyntaxType.PropertySyntax:
                                return new PropertySyntaxDescription();
                        }
                    }
                }

                return new SyntaxDescription();
            }
        }

        public abstract class InheritClassJsonConverter<T> : JsonConverter
        {
            protected abstract object CreateInheritClass(JObject jsonObject);

            public override bool CanConvert(Type objectType)
            {
                if (objectType.IsA(typeof(T)))
                {
                    return true;
                }

                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject objectJ = JObject.Load(reader);
                var a = this.CreateInheritClass(objectJ);
                serializer.Populate(objectJ.CreateReader(), a);
                return a;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override bool CanWrite
            {
                get
                {
                    return false;
                }
            }
        }
    }
}
