// <copyright file="AssemblyExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using Collections;

    using JetBrains.Annotations;

    /// <summary>
    /// The assembly extensions class
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Loads the configuration from assembly attributes
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to find.</typeparam>
        /// <param name="callingAssembly">The calling assembly to search.</param>
        /// <returns>The custom attribute of type T, if found.</returns>
        [CanBeNull]
        public static T GetAttribute<T>([NotNull] this Assembly callingAssembly)
            where T : Attribute
        {
            Contract.Requires(callingAssembly != null);

            T result = null;

            // Try to find the configuration attribute for the default logger if it exists
            var configAttributes = Attribute.GetCustomAttributes(callingAssembly, typeof(T), inherit: false);

            // get just the first one
            if (!configAttributes.IsNullOrEmpty())
            {
                result = (T)configAttributes[0];
            }

            return result;
        }
    }
}