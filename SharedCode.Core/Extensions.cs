// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// The extensions class.
    /// </summary>
    public static class Extensions
    {
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
    }
}
