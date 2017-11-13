// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFormatAttribute.cs" company="improvGroup, LLC">
// Copyright © 2005-2017 improvGroup, LLC. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SharedCode.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    /// <summary>
    ///     Specifies a custom format to use when serializing objects supporting IFormattable.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// If the type of the field/property implements IFormattable, this format will be used when writing.
        /// </param>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public DataFormatAttribute([NotNull] string format)
        {
            Contract.Requires(format != null);
            this.Format = format;
        }

        /// <summary>
        ///     Gets the format to use when serializing objects supporting IFormattable.
        /// </summary>
        /// <value>
        /// The format to use when serializing objects supporting IFormattable.
        /// </value>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [NotNull]
        public string Format { get; }
    }
}
