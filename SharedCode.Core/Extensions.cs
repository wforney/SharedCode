// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// The extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The XML serializers
        /// </summary>
        private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> XmlSerializers = new Dictionary<RuntimeTypeHandle, XmlSerializer>();

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is between the <paramref name="from"/> and <paramref name="to"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared.</typeparam>
        /// <param name="value">The value to be compared.</param>
        /// <param name="from">The lower bound value.</param>
        /// <param name="to">The upper bound value.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is between the <paramref name="from"/> and <paramref name="to"/> values, <c>false</c> otherwise.</returns>
        public static bool Between<T>(this T value, T from, T to) where T : IComparable<T> => value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;

        /// <summary>
        /// Makes a copy from the object.
        /// Doesn't copy the reference memory, only data.
        /// </summary>
        /// <typeparam name="T">Type of the return object.</typeparam>
        /// <param name="item">Object to be copied.</param>
        /// <returns>Returns the copied object.</returns>
        public static T Clone<T>(this object item)
        {
            if (item == null)
            {
                return default;
            }

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)new BinaryFormatter().Deserialize(stream);
            }
        }

        /// <summary>
        /// Converts the value to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The input value.</param>
        /// <returns>The output value.</returns>
        public static T ConvertTo<T>(this IConvertible value)
            => (T)Convert.ChangeType(value, typeof(T));

        /// <summary>
        /// Converts the value to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="value">The input value.</param>
        /// <returns>The output value.</returns>
        public static T ConvertTo<T>(this object value)
            => (T)Convert.ChangeType(value, typeof(T));

        /// <summary>
        /// Converts a JSON string to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object represented in the JSON string.</typeparam>
        /// <param name="jsonString">The JSON string.</param>
        /// <returns>The output.</returns>
        public static T FromJson<T>(this object jsonString) => JsonConvert.DeserializeObject<T>(jsonString as string);

        /// <summary>
        /// If the object this method is called on is not null, runs the given function and returns the value.
        /// If the object is null, returns default(TResult)
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="getValue">The get value function.</param>
        /// <returns>The result.</returns>
        public static TResult IfNotNull<T, TResult>(this T target, Func<T, TResult> getValue)
        {
            var handler = getValue;
            return handler == null || EqualityComparer<T>.Default.Equals(target, default) ? default : handler(target);
        }

        /// <summary>
        /// Determines whether the specified value is between the low and high values.
        /// </summary>
        /// <typeparam name="T">The types being compared.</typeparam>
        /// <param name="value">The value to compare.</param>
        /// <param name="low">The low value.</param>
        /// <param name="high">The high value.</param>
        /// <returns><c>true</c> if the specified value is between the low and high values; otherwise, <c>false</c>.</returns>
        public static bool IsBetween<T>(this T value, T low, T high)
                where T : IComparable<T>
                => value.CompareTo(low) >= 0 && value.CompareTo(high) <= 0;

        /// <summary>
        /// Determines whether the specified source object is not null.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns><c>true</c> if the specified source object is not null; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull(this object source) => source != null;

        /// <summary>
        /// Determines whether the specified source object is null.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns><c>true</c> if the specified source object is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull(this object source) => source == null;

        /// <summary>
        /// Convert an object to JSON.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns>The JSON representation of the object.</returns>
        public static string ToJson(this object input) => JsonConvert.SerializeObject(input);

        /// <summary>
        /// Serialize object to xml string by <see cref="XmlSerializer" />
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="value">The input value.</param>
        /// <returns>The XML representation of the input value.</returns>
        public static string ToXml<T>(this T value)
            where T : new()
        {
            var serializer = GetXmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            using (var writer = new XmlTextWriter(stream, new UTF8Encoding()))
            {
                serializer.Serialize(writer, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Serialize object to xml string by <see cref="XmlSerializer" />
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="value">The input value.</param>
        /// <param name="stream">The output stream.</param>
        public static void ToXml<T>(this T value, Stream stream)
            where T : new()
        {
            var serializer = GetXmlSerializer(typeof(T));
            serializer.Serialize(stream, value);
        }

        /// <summary>
        /// Serializes the input object to an XML string.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns>The XML string.</returns>
        public static string ToXml(this object input)
        {
            // Serialize an object into an xml string
            var xmlSerializer = new XmlSerializer(input.GetType());

            // use new UTF8Encoding here, not Encoding.UTF8.
            using (var memoryStream = new MemoryStream())
            using (var xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding()))
            {
                xmlSerializer.Serialize(xmlTextWriter, input);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Gets the XML serializer for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type handled by the serializer.</param>
        /// <returns>The <see cref="XmlSerializer"/> for the <paramref name="type"/>.</returns>
        private static XmlSerializer GetXmlSerializer(Type type)
        {
            if (!XmlSerializers.TryGetValue(type.TypeHandle, out var serializer))
            {
                lock (XmlSerializers)
                {
                    if (!XmlSerializers.TryGetValue(type.TypeHandle, out serializer))
                    {
                        serializer = new XmlSerializer(type);
                        XmlSerializers.Add(type.TypeHandle, serializer);
                    }
                }
            }
            return serializer;
        }
    }
}
