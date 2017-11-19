// <copyright file="TypeExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    ///     The type extensions class
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Loads the custom attributes from the type
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to find.</typeparam>
        /// <param name="typeWithAttributes">The calling assembly to search.</param>
        /// <returns>The custom attribute of type T, if found.</returns>
        [NotNull]
        public static T GetAttribute<T>([NotNull] this Type typeWithAttributes)
            where T : Attribute => typeWithAttributes.GetAttributes<T>().FirstOrDefault();

        /// <summary>
        /// Loads the custom attributes from the type
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to find.</typeparam>
        /// <param name="typeWithAttributes">The calling assembly to search.</param>
        /// <returns>An enumeration of attributes of type T that were found.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<T> GetAttributes<T>([NotNull] this Type typeWithAttributes)
            where T : Attribute
        {
            Contract.Requires(typeWithAttributes != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            // Try to find the configuration attribute for the default logger if it exists
            var configAttributes = Attribute.GetCustomAttributes(typeWithAttributes, typeof(T), inherit: false);
            if (configAttributes == null)
            {
                yield break;
            }

            foreach (var configAttribute in configAttributes)
            {
                var attribute = (T)configAttribute;
                yield return attribute;
            }
        }

        /// <summary>
        /// Determines whether the specified type is boolean.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <returns><c>true</c> if the specified type is boolean; otherwise, <c>false</c>.</returns>
        public static bool IsBoolean([CanBeNull] this Type type) => type == typeof(bool);

        /// <summary>
        ///     Return true if the type is a System.Nullable wrapper of a value type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is a System.Nullable wrapper</returns>
        /// <exception cref="InvalidOperationException">
        ///     The current type is not a generic type.  That is,
        ///     <see cref="P:System.Type.IsGenericType"></see> returns false.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The invoked method is not supported in the base class. Derived classes must
        ///     provide an implementation.
        /// </exception>
        public static bool IsNullable([NotNull] this Type type)
        {
            Contract.Requires(type != null);

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether the specified type is string.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type is string; otherwise, <c>false</c>.</returns>
        public static bool IsString([NotNull] this Type type) => type == typeof(string);

        /// <summary>
        /// Alternative version of <see cref="Type.IsSubclassOf"/> that supports raw generic types (generic types without
        /// any type parameters).
        /// </summary>
        /// <param name="baseType">The base type class for which the check is made.</param>
        /// <param name="toCheck">To type to determine for whether it derives from <paramref name="baseType"/>.</param>
        public static bool IsSubclassOfRawGeneric([CanBeNull] this Type toCheck, [CanBeNull] Type baseType)
        {
            if (toCheck == null || baseType == null)
            {
                return false;
            }

            while (toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (baseType == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}