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
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    ///     The enumerable extensions class.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Gets the random.
        /// </summary>
        /// <value>The random.</value>
        [NotNull] private static readonly Random Random = new Random();

        /// <summary>
        ///     Aggregates the source.
        /// </summary>
        /// <typeparam name="T">The type of the items in the source.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="aggregateFunction">The aggregate function.</param>
        /// <returns>The result.</returns>
        [CanBeNull]
        public static T Aggregate<T>(
            [ItemCanBeNull] [CanBeNull] this IEnumerable<T> source,
            [NotNull] Func<T, T, T> aggregateFunction)
            => source.Aggregate(default, aggregateFunction);

        /// <summary>
        ///     Aggregates the source.
        /// </summary>
        /// <typeparam name="T">The type of the items in the source.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="aggregateFunction">The aggregate function.</param>
        /// <returns>The result.</returns>
        [CanBeNull]
        public static T Aggregate<T>(
            [ItemCanBeNull] [CanBeNull] this IEnumerable<T> source,
            [CanBeNull] T defaultValue,
            [NotNull] Func<T, T, T> aggregateFunction)
            => source?.Any() ?? false ? source.Aggregate(aggregateFunction) : defaultValue;

        /// <summary>
        ///     Returns a lazy evaluated enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The results.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<T> Cache<T>([NotNull] [ItemNotNull] this IEnumerable<T> source)
            => EnumerableExtensions.CacheHelper(source.GetEnumerator());

        /// <summary>
        ///     Returns all combinations of a chosen amount of selected elements in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequence.</typeparam>
        /// <param name="source">The source for this extension method.</param>
        /// <param name="select">The amount of elements to select for every combination.</param>
        /// <param name="repetition">True when repetition of elements is allowed.</param>
        /// <returns>All combinations of a chosen amount of selected elements in the sequence.</returns>
        [ItemCanBeNull]
        [NotNull]
        public static IEnumerable<IEnumerable<T>> Combinations<T>(
            [ItemCanBeNull] [NotNull] this IEnumerable<T> source,
            int select,
            bool repetition = false)
        {
            Contract.Requires(source != null);
            Contract.Requires(select >= 0);
            Contract.Ensures(Contract.Result<IEnumerable<IEnumerable<T>>>() != null);

            if (select == 0)
            {
                return new[] { new T[0] };
            }

            return source.SelectMany(
                (element, index)
                    =>
                        source
                            .Skip(repetition ? index : index + 1)
                            .Combinations(select - 1, repetition)
                            .Select(c => new[] { element }.Concat(c)));
        }

        /// <summary>
        ///     Provides a Distinct method that takes a key selector lambda as parameter.
        ///     The .NET framework only provides a Distinct method that takes an instance of an implementation
        ///     of <see cref="IEqualityComparer{T}" /> where the standard parameterless Distinct that uses
        ///     the default equality comparer doesn't suffice.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>The enumerable.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<T> Distinct<T, TKey>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<T> source,
            [NotNull] Func<T, TKey> keySelector)
            => source?.GroupBy(keySelector).Select(grps => grps).Select(e => e.First());

        /// <summary>
        ///     For each item in the enumerable performs the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="action">The action to be performed on each item.</param>
        /// <returns>The enumerable.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<T> ForEach<T>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<T> source,
            [NotNull] Action<T> action)
        {
            return source?.Select(
                item =>
                {
                    action?.Invoke(item);
                    return item;
                });
        }

        /// <summary>
        ///     For each item in the enumerable performs the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="action">The action to be performed on each item.</param>
        /// <returns>The enumerable.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<T> ForEach<T>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable source,
            [NotNull] Action<T> action) => source?.Cast<T>().ForEach(action);

        /// <summary>
        ///     For each item in the enumerable performs the specified action.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <typeparam name="TResult">The type of the items in the result.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="function">The function to be executed on each item.</param>
        /// <returns>The enumerable.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<TResult> ForEach<T, TResult>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<T> source,
            [NotNull] Func<T, TResult> function)
            => source?.Select(function);

        /// <summary>
        ///     Returns the index of the first occurrence in a sequence by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<TSource>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> source,
            [CanBeNull] TSource value) where TSource : IEquatable<TSource>
            => source.IndexOf(value, EqualityComparer<TSource>.Default);

        /// <summary>
        ///     Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<TSource>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> source,
            [CanBeNull] TSource value,
            [NotNull] IEqualityComparer<TSource> comparer)
        {
            Contract.Requires(comparer != null);

            if (source == null)
            {
                return -1;
            }

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
        ///     Determines whether the source enumerable is not null and contains items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns><c>true</c> if the source enumerable is not null and contains items; otherwise, <c>false</c>.</returns>
        public static bool IsNotNullOrEmpty<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
            => source?.Any() == true;

        /// <summary>
        ///     Determines whether the source enumerable is null or contains no items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns><c>true</c> if the source enumerable is null or contains no items; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
            => !source.IsNotNullOrEmpty();

        /// <summary>
        ///     Sorts the specified enumerable by the sort expression.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns>The sorted enumerable.</returns>
        /// <exception cref="Exception">No property x in type T.</exception>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<T> OrderBy<T>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<T> source,
            [CanBeNull] string sortExpression)
        {
            sortExpression += string.Empty;
            var parts = sortExpression.Split(' ');
            var descending = false;

            if (parts.Length <= 0 || parts[0] == string.Empty)
            {
                return source;
            }

            var property = parts[0];

            if (parts.Length > 1)
            {
                descending = parts[1].IndexOf("esc", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            var prop = typeof(T).GetProperty(property);

            if (prop == null)
            {
                throw new Exception($"No property '{property}' in {typeof(T).Name}");
            }

            return descending
                ? source.OrderByDescending(x => prop.GetValue(x, null))
                : source.OrderBy(x => prop.GetValue(x, null));
        }

        /// <summary>
        ///     Sorts the specified enumerable by the key selector function in the direction specified.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="descending">if set to <c>true</c> the sort direction is descending.</param>
        /// <returns>The sorted enumerable.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> enumerable,
            [NotNull] Func<TSource, TKey> keySelector,
            bool descending)
        {
            Contract.Requires(keySelector != null);

            if (enumerable == null)
            {
                return Enumerable.Empty<TSource>().OrderBy(i => i);
            }

            return descending ? enumerable.OrderByDescending(keySelector) : enumerable.OrderBy(keySelector);
        }

        /// <summary>
        ///     Sorts the specified enumerable by the key selector function in the direction specified.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="keySelector1">The key first selector.</param>
        /// <param name="keySelector2">The key second selector.</param>
        /// <param name="keySelectors">The remaining key selectors.</param>
        /// <returns>The sorted enumerable.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source">source</paramref> or
        ///     <paramref name="keySelectors">keySelector</paramref> is null.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The array is multidimensional and contains more than
        ///     <see cref="F:System.Int32.MaxValue"></see> elements.
        /// </exception>
        [ItemCanBeNull]
        [CanBeNull]
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, IComparable> keySelector1,
            [NotNull] Func<TSource, IComparable> keySelector2,
            [CanBeNull] [ItemNotNull] params Func<TSource, IComparable>[] keySelectors)
        {
            Contract.Requires(keySelector1 != null);
            Contract.Requires(keySelector2 != null);

            if (source == null)
            {
                return Enumerable.Empty<TSource>().OrderBy(i => i);
            }

            var current = source;

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
        ///     Sorts the specified enumerable by the key selector function in the direction specified.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="descending">if set to <c>true</c> the sort direction is descending.</param>
        /// <param name="keySelectors">The remaining key selectors.</param>
        /// <returns>The sorted enumerable.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> enumerable,
            [NotNull] Func<TSource, IComparable> keySelector,
            bool descending,
            [CanBeNull] [ItemNotNull] params Func<TSource, IComparable>[] keySelectors)
        {
            Contract.Requires(keySelector != null);

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
        ///     Groups the elements of a sequence according to a specified firstKey selector function and rotates the unique values
        ///     from the secondKey selector function into multiple values in the output, and performs aggregations.
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
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [NotNull]
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<
            TSource, TFirstKey, TSecondKey, TValue>(
            [NotNull] [ItemCanBeNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TFirstKey> firstKeySelector,
            [NotNull] Func<TSource, TSecondKey> secondKeySelector,
            [CanBeNull] Func<IEnumerable<TSource>, TValue> aggregate)
        {
            Contract.Requires(source != null);
            Contract.Requires(firstKeySelector != null);
            Contract.Requires(secondKeySelector != null);
            Contract.Ensures(Contract.Result<Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>>() != null);

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

#pragma warning disable SG0005 // Weak random generator

        /// <summary>
        ///     Randomizes order of the items in the specified enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The enumerable with random order applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        [CanBeNull]
        [ItemCanBeNull]
        public static IOrderedEnumerable<T> Randomize<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
            => source?.OrderBy(x => EnumerableExtensions.Random.Next());

#pragma warning restore SG0005 // Weak random generator

        /// <summary>
        ///     Slices the source from specified start to the specified end.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumeration.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <returns>The slice.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<T> Slice<T>([NotNull] [ItemCanBeNull] this IEnumerable<T> source, int start, int end)
        {
            Contract.Requires(source != null);

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var index = 0;

            // Optimise item count for ICollection interfaces.
            var count = source is ICollection<T> collection
                ? collection.Count
                : (source as ICollection)?.Count ?? source.Count();

            // Get start/end indexes, negative numbers start at the end of the collection.
            if (start < 0)
            {
                start += count;
            }

            if (end < 0)
            {
                end += count;
            }

            foreach (var item in source)
            {
                if (index >= end)
                {
                    yield break;
                }

                if (index >= start)
                {
                    yield return item;
                }

                ++index;
            }
        }

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static double StdDev([NotNull] this IEnumerable<int> source) => source.StdDevLogic();

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static double StdDev([NotNull] this IEnumerable<double> source) => source.StdDevLogic();

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static float StdDev([NotNull] this IEnumerable<float> source) => source.StdDevLogic();

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static double StdDevP([NotNull] this IEnumerable<int> source) => source.StdDevLogic(0);

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static double StdDevP([NotNull] this IEnumerable<double> source) => source.StdDevLogic(0);

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static double StdDevP([NotNull] this IEnumerable<float> source) => source.StdDevLogic(0);

        /// <summary>
        ///     Converts the source enumerable to a new collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The output collection.</returns>
        [NotNull]
        [ItemCanBeNull]
        public static Collection<T> ToCollection<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
        {
            Contract.Ensures(Contract.Result<Collection<T>>() != null);

            var collection = new Collection<T>();
            if (source != null)
            {
                foreach (var item in source)
                {
                    collection.Add(item);
                }
            }

            return collection;
        }

        /// <summary>
        ///     Returns a comma delimited string representing the values in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The delimited string.</returns>
        [CanBeNull]
        public static string ToCommaSeparatedValueString<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
            => source?.ToDelimitedString(',');

        /// <summary>
        ///     Converts an enumerable to a data table.
        /// </summary>
        /// <typeparam name="T">The type of items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The data table.</returns>
        [NotNull]
        public static DataTable ToDataTable<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
        {
            Contract.Ensures(Contract.Result<DataTable>() != null);

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
                    oProps = rec?.GetType().GetProperties();
                    if (oProps != null)
                    {
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;

                            if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
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
                }

                var dr = dtReturn.NewRow();

                if (oProps != null)
                {
                    foreach (var pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        /// <summary>
        ///     Returns a delimited string representing the values in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="separator">The character separator.</param>
        /// <returns>The delimited string.</returns>
        [CanBeNull]
        public static string ToDelimitedString<T>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<T> source,
            char separator)
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
        ///     Converts an enumeration of groupings into a Dictionary of those groupings.
        /// </summary>
        /// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
        /// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
        /// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
        /// <returns>
        ///     A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue
        ///     type.
        /// </returns>
        [CanBeNull]
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings?.ToDictionary(
                grouping => grouping == null ? default : grouping.Key,
                values => values?.ToList() ?? new List<TValue>());
        }

        /// <summary>
        ///     Converts an IEnumerable to a HashSet
        /// </summary>
        /// <typeparam name="T">The IEnumerable type</typeparam>
        /// <param name="enumerable">The IEnumerable</param>
        /// <returns>A new HashSet</returns>
        [NotNull]
        [ItemCanBeNull]
        public static HashSet<T> ToHashSet<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> enumerable)
        {
            Contract.Ensures(Contract.Result<HashSet<T>>() != null);

            var hs = new HashSet<T>();
            if (enumerable != null)
            {
                foreach (var item in enumerable)
                {
                    hs.Add(item);
                }
            }

            return hs;
        }

        /// <summary>
        ///     Converts the enumerable to an observable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <returns>The observable collection.</returns>
        [NotNull]
        [ItemCanBeNull]
        public static ObservableCollection<T> ToObservableCollection<T>(
            [NotNull] [ItemCanBeNull] this IEnumerable<T> source) => new ObservableCollection<T>().AddRange(source);

        /// <summary>
        ///     Transposes the rows and columns of the specified nested enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="values">The input values.</param>
        /// <returns>The transposed output.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<IEnumerable<T>> Transpose<T>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<IEnumerable<T>> values)
        {
            while (true)
            {
                if (values == null || !values.Any())
                {
                    return Enumerable.Empty<IEnumerable<T>>();
                }

                if (!values.First().Any())
                {
                    values = values.Skip(1);
                    continue;
                }

                var val = values.First().First();
                var valSkip = values.First().Skip(1);
                var xss = values.Skip(1);
                var xssList = xss as IList<IEnumerable<T>> ?? xss.ToList();
                return new[]
                {
                    new[] {val}.Concat(
                        xssList.Select(ht => ht.First()))
                }.Concat(
                    new[] { valSkip }.Concat(xssList.Select(ht => ht.Skip(1)))
                                   .Transpose());
            }
        }

        /// <summary>
        ///     When building a LINQ query, you may need to involve optional filtering criteria.
        ///     Avoids if statements when building predicates &amp; lambdas for a query.
        ///     Useful when you don't know at compile time whether a filter should apply.
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="predicate">The predicate for the where clause.</param>
        /// <param name="condition">If evaluates to <c>true</c> then apply the predicate, else just return the enumerable.</param>
        /// <returns>The enumerable with the predicate applied if condition evaluated to true.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<TSource> WhereIf<TSource>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, bool> predicate,
            bool condition)
            => condition ? source.Where(predicate) : source;

        /// <summary>
        ///     When building a LINQ query, you may need to involve optional filtering criteria.
        ///     Avoids if statements when building predicates &amp; lambdas for a query.
        ///     Useful when you don't know at compile time whether a filter should apply.
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="predicate">The predicate for the where clause.</param>
        /// <param name="condition">If evaluates to <c>true</c> then apply the predicate, else just return the enumerable.</param>
        /// <returns>The enumerable with the predicate applied if condition evaluated to true.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        public static IEnumerable<TSource> WhereIf<TSource>(
            [CanBeNull] [ItemCanBeNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, int, bool> predicate,
            bool condition)
            => condition ? source.Where(predicate) : source;

        /// <summary>
        ///     Returns a lazy evaluated enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns>The results.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        private static IEnumerable<T> CacheHelper<T>([NotNull] IEnumerator<T> source)
        {
            Contract.Requires(source != null);

            var isEmpty = new Lazy<bool>(() => !source.MoveNext());
            var head = new Lazy<T>(() => source.Current);
            var tail = new Lazy<IEnumerable<T>>(() => EnumerableExtensions.CacheHelper(source));

            return EnumerableExtensions.CacheHelper(isEmpty, head, tail);
        }

        /// <summary>
        ///     Returns a lazy evaluated enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="isEmpty">Whether the enumerable is empty.</param>
        /// <param name="head">The head of the enumerable.</param>
        /// <param name="tail">The tail of the enumerable.</param>
        /// <returns>The results.</returns>
        [CanBeNull]
        [ItemCanBeNull]
        private static IEnumerable<T> CacheHelper<T>(
            [NotNull] Lazy<bool> isEmpty,
            [NotNull] [ItemCanBeNull] Lazy<T> head,
            [NotNull] [ItemCanBeNull] Lazy<IEnumerable<T>> tail)
        {
            Contract.Requires(isEmpty != null);
            Contract.Requires(head != null);
            Contract.Requires(tail != null);

            if (isEmpty.Value)
            {
                yield break;
            }

            yield return head.Value;

            if (tail.Value == null)
            {
                yield break;
            }

            foreach (var value in tail.Value)
            {
                yield return value;
            }
        }

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <param name="buffer">The buffer amount.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        private static double StdDevLogic([NotNull] this IEnumerable<double> source, int buffer = 1)
        {
            Contract.Requires(source != null);
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var data = source.ToList();
            var average = data.Average();
            var differences = data.Select(u => Math.Pow(average - u, 2.0)).ToList();
            return Math.Sqrt(differences.Sum() / (differences.Count - buffer));
        }

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <param name="buffer">The buffer amount.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        /// ReSharper disable once SuspiciousTypeConversion.Global
        private static double StdDevLogic([NotNull] this IEnumerable<int> source, int buffer = 1)
            => source.Cast<double>().StdDevLogic(buffer);

        /// <summary>
        ///     Typical standard deviation formula set in LINQ fluent syntax. For when Average, Min, and Max just aren't enough
        ///     information. Works with int, double, float.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <param name="buffer">The buffer amount.</param>
        /// <returns>The standard deviation.</returns>
        /// <exception cref="ArgumentNullException">source</exception>
        private static float StdDevLogic([NotNull] this IEnumerable<float> source, int buffer = 1)
        {
            Contract.Requires(source != null);
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var data = source.ToList();
            var average = data.Average();
            var differences = data.Select(u => Math.Pow(average - u, 2.0)).ToList();
            return (float)Math.Sqrt(differences.Sum() / (differences.Count - buffer));
        }
    }
}