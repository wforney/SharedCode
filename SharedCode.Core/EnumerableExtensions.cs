// <copyright file="EnumerableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// The enumerable extensions class.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets the random.
        /// </summary>
        /// <value>The random.</value>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Returns a lazy evaluated enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The results.</returns>
        public static IEnumerable<T> Cache<T>(this IEnumerable<T> source) => CacheHelper(source.GetEnumerator());

        /// <summary>
        /// Provides a Distinct method that takes a key selector lambda as parameter.
        /// The .NET framework only provides a Distinct method that takes an instance of an implementation
        /// of <see cref="IEqualityComparer{T}"/> where the standard parameterless Distinct that uses
        /// the default equality comparer doesn't suffice.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>The enumerable.</returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
            => source.GroupBy(keySelector).Select(grps => grps).Select(e => e.First());

        /// <summary>
        /// For each item in the enumerable performs the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="action">The action to be performed on each item.</param>
        /// <returns>The enumerable.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            return source.Select(
                item =>
                {
                    action?.Invoke(item);
                    return item;
                });
        }

        /// <summary>
        /// For each item in the enumerable performs the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="action">The action to be performed on each item.</param>
        /// <returns>The enumerable.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable source, Action<T> action) => source.Cast<T>().ForEach(action);

        /// <summary>
        /// For each item in the enumerable performs the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <typeparam name="TResult">The type of the items in the result.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="function">The function to be executed on each item.</param>
        /// <returns>The enumerable.</returns>
        public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> source, Func<T, TResult> function) => source.Select(function);

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value) where TSource : IEquatable<TSource>
            => source.IndexOf(value, EqualityComparer<TSource>.Default);

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Determines whether the source enumerable is not null and contains items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns><c>true</c> if the source enumerable is not null and contains items; otherwise, <c>false</c>.</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source) => source?.Any() == true;

        /// <summary>
        /// Determines whether the source enumerable is null or contains no items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns><c>true</c> if the source enumerable is null or contains no items; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => !source.IsNotNullOrEmpty();

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
        /// Sorts the specified enumerable by the key selector function in the direction specified.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="descending">if set to <c>true</c> the sort direction is descending.</param>
        /// <returns>The sorted enumerable.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, bool descending)
        {
            if (enumerable == null)
            {
                return Enumerable.Empty<TSource>().OrderBy(i => i);
            }

            return descending ? enumerable.OrderByDescending(keySelector) : enumerable.OrderBy(keySelector);
        }

        /// <summary>
        /// Sorts the specified enumerable by the key selector function in the direction specified.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <param name="keySelector1">The key first selector.</param>
        /// <param name="keySelector2">The key second selector.</param>
        /// <param name="keySelectors">The remaining key selectors.</param>
        /// <returns>The sorted enumerable.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(
            this IEnumerable<TSource> enumerable,
            Func<TSource, IComparable> keySelector1,
            Func<TSource, IComparable> keySelector2,
            params Func<TSource, IComparable>[] keySelectors)
        {
            if (enumerable == null)
            {
                return Enumerable.Empty<TSource>().OrderBy(i => i);
            }

            var current = enumerable;

            if (keySelectors != null)
            {
                for (var i = keySelectors.Length - 1; i >= 0; i--)
                {
                    current = current.OrderBy(keySelectors[i]);
                }
            }

            current = current.OrderBy(keySelector2);

            return current.OrderBy(keySelector1);
        }

        /// <summary>
        /// Sorts the specified enumerable by the key selector function in the direction specified.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="descending">if set to <c>true</c> the sort direction is descending.</param>
        /// <param name="keySelectors">The remaining key selectors.</param>
        /// <returns>The sorted enumerable.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(
            this IEnumerable<TSource> enumerable,
            Func<TSource, IComparable> keySelector,
            bool descending,
            params Func<TSource, IComparable>[] keySelectors)
        {
            if (enumerable == null)
            {
                return Enumerable.Empty<TSource>().OrderBy(i => i);
            }

            var current = enumerable;

            if (keySelectors != null)
            {
                for (var i = keySelectors.Length - 1; i >= 0; i--)
                {
                    current = current.OrderBy(keySelectors[i], descending);
                }
            }

            return current.OrderBy(keySelector, descending);
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
        /// Randomizes order of the items in the specified enumerable.
        /// </summary>
        /// <typeparam name="t">The type of the items in the enumerable.</typeparam>
        /// <param name="target">The enumerable.</param>
        /// <returns>The enumerable with random order applied.</returns>
#pragma warning disable SG0005 // Weak random generator
        public static IOrderedEnumerable<t> Randomize<t>(this IEnumerable<t> target) => target.OrderBy(x => Random.Next());
#pragma warning restore SG0005 // Weak random generator

        /// <summary>
        /// Converts the source enumerable to a new collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The output collection.</returns>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            var collection = new Collection<T>();
            foreach (var item in source)
            {
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>
        /// Returns a comma delimited string representing the values in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The delimited string.</returns>
        public static string ToCommaSeparatedValueString<T>(this IEnumerable<T> source) => source.ToDelimitedString(',');

        /// <summary>
        /// Converts an enumerable to a data table.
        /// </summary>
        /// <typeparam name="T">The type of items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The data table.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            var dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (source == null)
            {
                return dtReturn;
            }

            foreach (var rec in source)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

#pragma warning disable DF0000 // Marks undisposed anonymous objects from object creations.
#pragma warning disable CC0022 // Should dispose object
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
#pragma warning restore CC0022 // Should dispose object
#pragma warning restore DF0000 // Marks undisposed anonymous objects from object creations.
                    }
                }

                var dr = dtReturn.NewRow();

                foreach (var pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        /// <summary>
        /// Returns a delimited string representing the values in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="separator">The character separator.</param>
        /// <returns>The delimited string.</returns>
        public static string ToDelimitedString<T>(this IEnumerable<T> source, char separator)
        {
            if (source == null)
            {
                return null;
            }

            var result = new StringBuilder();
            source.ForEach(value => result.AppendFormat("{0}{1}", value, separator));
            return result.ToString(0, result.Length - 1);
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

        /// <summary>
        /// Converts the enumerable to an observable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The observable collection.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source) => new ObservableCollection<T>().AddRange(source);

        /// <summary>
        /// Transposes the rows and columns of the specified nested enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="values">The input values.</param>
        /// <returns>The transposed output.</returns>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values)
        {
            if (!values.Any())
            {
                return values;
            }

            if (!values.First().Any())
            {
                return Transpose(values.Skip(1));
            }

            var val = values.First().First();
            var valSkip = values.First().Skip(1);
            var xss = values.Skip(1);
            return new[] {new[] {val}
                .Concat(
                    xss.Select(ht => ht.First()))}
                       .Concat(new[] { valSkip }
                       .Concat(xss.Select(ht => ht.Skip(1)))
                .Transpose());
        }

        /// <summary>
        /// When building a LINQ query, you may need to involve optional filtering criteria.
        /// Avoids if statements when building predicates &amp; lambdas for a query.
        /// Useful when you don't know at compile time whether a filter should apply.
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="predicate">The predicate for the where clause.</param>
        /// <param name="condition">If evaluates to <c>true</c> then apply the predicate, else just return the enumerable.</param>
        /// <returns>The enumerable with the predicate applied if condition evaluated to true.</returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool condition)
            => condition ? source.Where(predicate) : source;

        /// <summary>
        /// When building a LINQ query, you may need to involve optional filtering criteria.
        /// Avoids if statements when building predicates &amp; lambdas for a query.
        /// Useful when you don't know at compile time whether a filter should apply.
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="predicate">The predicate for the where clause.</param>
        /// <param name="condition">If evaluates to <c>true</c> then apply the predicate, else just return the enumerable.</param>
        /// <returns>The enumerable with the predicate applied if condition evaluated to true.</returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate, bool condition)
            => condition ? source.Where(predicate) : source;

        /// <summary>
        /// Returns a lazy evaluated enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The results.</returns>
        private static IEnumerable<T> CacheHelper<T>(IEnumerator<T> source)
        {
            var isEmpty = new Lazy<bool>(() => !source.MoveNext());
            var head = new Lazy<T>(() => source.Current);
            var tail = new Lazy<IEnumerable<T>>(() => CacheHelper(source));

            return CacheHelper(isEmpty, head, tail);
        }

        /// <summary>
        /// Returns a lazy evaluated enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="isEmpty">Whether the enumerable is empty.</param>
        /// <param name="head">The head of the enumerable.</param>
        /// <param name="tail">The tail of the enumerable.</param>
        /// <returns>The results.</returns>
        private static IEnumerable<T> CacheHelper<T>(
            Lazy<bool> isEmpty,
            Lazy<T> head,
            Lazy<IEnumerable<T>> tail)
        {
            if (isEmpty.Value)
            {
                yield break;
            }

            yield return head.Value;
            foreach (var value in tail.Value)
            {
                yield return value;
            }
        }
    }
}
