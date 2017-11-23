// <copyright file="EnumeratorExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Linq
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using JetBrains.Annotations;

    /// <summary>
    /// The enumerator extensions class.
    /// </summary>
    public static class EnumeratorExtensions
    {
        /// <summary>
        /// Returns an enumerable from the enumerator.
        /// </summary>
        /// <typeparam name="T">The type of the enumerated items.</typeparam>
        /// <param name="enumerator">The input enumerator.</param>
        /// <returns>The enumerable.</returns>
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<T> ToEnumerable<T>([CanBeNull] this IEnumerator<T> enumerator)
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            while (enumerator?.MoveNext() == true)
            {
                yield return enumerator.Current;
            }
        }
    }
}