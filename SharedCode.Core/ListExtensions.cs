// <copyright file="ListExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// The list extensions class
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Determines whether a list is not null or empty.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The list.</param>
        /// <returns><c>true</c> if this list is not null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNotNullOrEmpty<T>(this IList<T> items) => items?.Count > 0;

        /// <summary>
        /// Determines whether a list is null or empty.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The list.</param>
        /// <returns><c>true</c> if this list is null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty<T>(this IList<T> items) => items == null || items.Count == 0;
    }
}
