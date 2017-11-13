// <copyright file="StringValueAttribute.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Attributes
{
    using System;
    using System.Diagnostics.Contracts;
    using JetBrains.Annotations;

    /// <summary>
    /// The string value attribute class
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class StringValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value">The string value.</param>
        public StringValueAttribute([NotNull] string value)
        {
            Contract.Requires(value != null);
            this.Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        [NotNull]
        public string Value { get; }
    }
}
