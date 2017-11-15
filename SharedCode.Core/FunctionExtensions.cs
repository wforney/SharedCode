// <copyright file="FunctionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The function extensions class
    /// </summary>
    public static class FunctionExtensions
    {
        /// <summary>
        /// Memoizes the function.
        /// </summary>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function to memoize.</param>
        /// <returns>The memoized function.</returns>
        [NotNull]
        public static Func<T, TResult> Memoize<T, TResult>([NotNull] this Func<T, TResult> function)
        {
            Contract.Requires(function != null);
            Contract.Ensures(Contract.Result<Func<T, TResult>>() != null);

            var dictionary = new Dictionary<T, TResult>();
            return n =>
            {
                if (dictionary.ContainsKey(n))
                {
                    return dictionary[n];
                }

                var handler = function;
                if (handler == null)
                {
                    throw new ArgumentNullException(nameof(function));
                }

                var result = handler(n);
                dictionary.Add(n, result);
                return result;
            };
        }
    }
}