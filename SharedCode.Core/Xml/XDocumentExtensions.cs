// <copyright file="XDocumentExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Xml
{
    using System.Diagnostics.Contracts;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using JetBrains.Annotations;

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
        [CanBeNull]
        public static T Deserialize<T>([NotNull] this XDocument xmlDocument)
        {
            Contract.Requires(xmlDocument != null);

            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xmlDocument.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }
}