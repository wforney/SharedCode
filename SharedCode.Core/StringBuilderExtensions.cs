// <copyright file="StringBuilderExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

using System.Diagnostics.Contracts;
using System.Text;
using JetBrains.Annotations;

namespace SharedCode.Core
{
    /// <summary>
    /// The string builder extensions class
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the formatted string plus a line terminator to this string builder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The format arguments.</param>
        /// <returns>The string builder.</returns>
        [CanBeNull]
        public static StringBuilder AppendLineFormat([CanBeNull] this StringBuilder builder, [NotNull] string format, [NotNull][ItemCanBeNull] params object[] arguments)
        {
            Contract.Requires(format != null);
            Contract.Requires(arguments != null);

            builder?.AppendFormat(format, arguments);
            builder?.AppendLine();
            return builder;
        }
    }
}