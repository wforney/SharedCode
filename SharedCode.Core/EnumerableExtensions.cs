// <copyright file="EnumerableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The enumerable extensions class.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Sorts the specified enumerable by the sort expression.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns>The sorted enumerable.</returns>
        /// <exception cref="Exception">No property x in type T.</exception>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortExpression)
        {
            sortExpression += string.Empty;
            var parts = sortExpression.Split(' ');
            var descending = false;
            var property = string.Empty;

            if (parts.Length > 0 && parts[0] != string.Empty)
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].IndexOf("esc", StringComparison.OrdinalIgnoreCase) >= 0;
                }

                var prop = typeof(T).GetProperty(property);

                if (prop == null)
                {
                    throw new Exception($"No property '{property}' in {typeof(T).Name}");
                }

                return descending ? source.OrderByDescending(x => prop.GetValue(x, null)) : source.OrderBy(x => prop.GetValue(x, null));
            }

            return source;
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified firstKey selector function and rotates the unique values from the secondKey selector function into multiple values in the output, and performs aggregations.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TFirstKey">The type of the first key.</typeparam>
        /// <typeparam name="TSecondKey">The type of the second key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="firstKeySelector">The first key selector.</param>
        /// <param name="secondKeySelector">The second key selector.</param>
        /// <param name="aggregate">The aggregate function.</param>
        /// <returns>Dictionary&lt;TFirstKey, Dictionary&lt;TSecondKey, TValue&gt;&gt;.</returns>
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TFirstKey> firstKeySelector,
            Func<TSource, TSecondKey> secondKeySelector,
            Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            foreach (var item in source.ToLookup(firstKeySelector))
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                foreach (var subitem in item.ToLookup(secondKeySelector))
                {
                    var handler = aggregate;
                    if (handler != null)
                    {
                        dict.Add(subitem.Key, handler(subitem));
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Converts an enumeration of groupings into a Dictionary of those groupings.
        /// </summary>
        /// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
        /// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
        /// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
        /// <returns>A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue type.</returns>
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
            => groupings.ToDictionary(group => group.Key, group => group.ToList());
    }
}
