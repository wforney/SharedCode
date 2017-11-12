// <copyright file="XDocumentExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// The <see cref="XDocument"/> extensions class.
    /// </summary>
    public static class XDocumentExtensions
    {
        /// <summary>
        /// Deserializes the specified XML document.
        /// </summary>
        /// <typeparam name="T">The type represented in the XML document.</typeparam>
        /// <param name="xmlDocument">The XML document.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(this XDocument xmlDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xmlDocument.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
