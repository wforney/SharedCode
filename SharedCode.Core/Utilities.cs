using System.Diagnostics.Contracts;
// <copyright file="Utilities.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// The utilities class
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Swaps the left reference with the right reference.
        /// </summary>
        /// <typeparam name="T">The type of the references.</typeparam>
        /// <param name="left">The left reference.</param>
        /// <param name="right">The right reference.</param>
        public static void Swap<T>([NotNull] ref T left, [NotNull] ref T right)
        {
            Contract.Requires(!System.Collections.Generic.EqualityComparer<T>.Default.Equals(left, default));
            Contract.Requires(!System.Collections.Generic.EqualityComparer<T>.Default.Equals(right, default));

            var tmp = left;
            left = right;
            right = tmp;
        }

        /// <summary>
        /// Gets the constant name from value.
        /// </summary>
        /// <param name="type">The input type.</param>
        /// <param name="val">The input value.</param>
        /// <returns>The constant name.</returns>
        /// <remarks>
        /// See also <seealso cref="http://stackoverflow.com/a/10261848/1449056" />
        /// </remarks>
        [NotNull]
        public static string GetConstantNameFromValue([NotNull] Type type, [NotNull] object val)
        {
            Contract.Requires(type != null);
            Contract.Requires(val != null);
            Contract.Ensures(Contract.Result<string>() != null);

            // Gets all public and static fields
            // This tells it to get the fields from all base types as well
            // Go through the list and only pick out the constants
            foreach (var fi in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                // remove deprecated / obsolete fields/properties
                if (fi.GetCustomAttributes<ObsoleteAttribute>(inherit: true).Any())
                {
                    continue;
                }

                // IsLiteral determines if its value is written at
                //   compile time and not changeable
                // IsInitOnly determine if the field can be set
                //   in the body of the constructor
                // for C# a field which is readonly keyword would have both true
                //   but a const field would have only IsLiteral equal to true
                if (fi.IsLiteral && !fi.IsInitOnly)
                {
                    var value = fi.GetRawConstantValue();
                    if (value.Equals(val))
                    {
                        return fi.Name;
                    }
                }
            }

            return val.ToString();
        }
    }
}
