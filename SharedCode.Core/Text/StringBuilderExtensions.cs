// <copyright file="StringBuilderExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Text
{
    using System.Diagnostics.Contracts;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    /// The string builder extensions class
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the specified <paramref name="value"/> to this <see cref="StringBuilder"/> if the <paramref name="condition"/> is true.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="value">The string value.</param>
        /// <param name="condition">if set to <c>true</c> then append the string value to the builder.</param>
        /// <returns>The <see cref="StringBuilder"/>.</returns>
        [NotNull]
        public static StringBuilder AppendIf([NotNull] this StringBuilder builder, [CanBeNull] string value, bool condition)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<StringBuilder>() != null);

            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

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