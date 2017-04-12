using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;

namespace BachMaiCR.Web.Common
{
    /// <summary>
    /// Read/Write object graphs to/from json using NewtonSoft.Json
    /// </summary>
    /// Creator: giangpn
    /// DateCreate:190514
    public static class Json2
    {
        /// <summary>
        /// Get a serializer suited for two-way serialization of .NET types.
        /// </summary>
        /// <returns></returns>
        public static JsonSerializer Serializer(
            TypeNameHandling typeNameHandling = TypeNameHandling.Arrays,
            FormatterAssemblyStyle assemblyNameType = FormatterAssemblyStyle.Simple,
            NullValueHandling nullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling defaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling missingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling loopHandling = ReferenceLoopHandling.Ignore)
        {
            var result = new JsonSerializer
            {
                MissingMemberHandling = missingMemberHandling,
                NullValueHandling = nullValueHandling,
                ReferenceLoopHandling = loopHandling,
                DefaultValueHandling = defaultValueHandling,
                TypeNameHandling = typeNameHandling,
                TypeNameAssemblyFormat = assemblyNameType
            };
            result.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return result;
        }

        /// <summary>
        /// Get a serializer suited for one-way serialization for Javascript clients' consumption.
        /// </summary>
        /// <param name="ignoreDefaultValue">Ignore default value: true else false</param>
        /// <returns></returns>
        public static JsonSerializer JsSerializer(bool ignoreDefaultValue = true)
        {
            var result = new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.None,
            };
            if (ignoreDefaultValue)
            {
                result.DefaultValueHandling = DefaultValueHandling.Ignore;
            }
            result.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            result.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return result;
        }

        /// <summary>
        /// Serialize the graph into Json without type information (suited for Javascript client).
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="ignoreDefaultValue">Ignore default value: true else false</param>
        /// <returns></returns>
        public static string StringifyJs(this object graph, bool ignoreDefaultValue = true)
        {
            using (var writer = new StringWriter())
            {
                JsSerializer(ignoreDefaultValue).Serialize(writer, graph);
                return writer.ToString();

            }
        }

        /// <summary>
        /// Remove type information for a json previously created for .NET.
        /// </summary>
        /// <param name="dotNetJson">The dot net json.</param>
        /// <returns></returns>
        public static string SanitizeDotNetJsonForJs(string dotNetJson)
        {
            return ParseAs<IDictionary<string, object>>(dotNetJson).StringifyJs();
        }

        /// <summary>
        /// Stringifies the specified graph with type information (suitable for two-way serialization of complex .NET types).
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="auto">if set to <c>true</c> [auto].</param>
        /// <returns></returns>
        public static string Stringify(this object graph, bool auto = false)
        {
            using (var writer = new StringWriter())
            {
                Stringify(graph, writer, auto);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Parses the input json string as an instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T ParseAs<T>(string json) where T : class
        {
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("json");
            }
            using (var reader = new StringReader(json))
            {
                return Parse(reader, typeof(T)) as T;
            }
        }

        /// <summary>
        /// Parses as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static T ParseAs<T>(TextReader reader) where T : class
        {
            return Parse(reader, typeof(T)) as T;
        }

        /// <summary>
        /// Parses the input json string as an instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStream">The json stream.</param>
        /// <returns></returns>
        public static T ParseAs<T>(Stream jsonStream) where T : class
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }
            using (var reader = new StreamReader(jsonStream))
            {
                return Parse(reader, typeof(T)) as T;
            }
        }

        /// <summary>
        /// Saves an object graph to a file as Json.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="file">The file.</param>
        public static void SaveToFile(object graph, string file)
        {
            if (String.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException("file");
            }
            using (var writer = new StreamWriter(file, false, Encoding.UTF8))
            using (var jsWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented })
            {
                var serializer = new JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Auto,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    ConstructorHandling = ConstructorHandling.Default,
                };
                serializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                serializer.Serialize(jsWriter, graph);
            }
        }

        /// <summary>
        /// Loads an object graph (of T) from a json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static T LoadFromFile<T>(string file) where T : class
        {
            if (String.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException("file");
            }
            using (var reader = new StreamReader(file, Encoding.UTF8, true))
            {
                return Parse(reader, typeof(T)) as T;
            }
        }

        /// <summary>
        /// Stringifies the specified graph to a text writer.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="auto">if set to <c>true</c> [auto].</param>
        public static void Stringify(object graph, TextWriter writer, bool auto = false)
        {
            var typeNameHandling = auto ? TypeNameHandling.Auto : TypeNameHandling.Arrays;
            Serializer(typeNameHandling).Serialize(writer, graph);
        }

        /// <summary>
        /// Parses the specified reader and create an object graph of the provided type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object Parse(TextReader reader, Type type)
        {
            return Serializer().Deserialize(reader, type);
        }
    }
}