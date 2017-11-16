// <copyright file="TypeExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using JetBrains.Annotations;

    /// <summary>
    ///     The type extensions class
    /// </summary>
    public static class TypeExtensions
    {
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