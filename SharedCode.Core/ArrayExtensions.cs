// <copyright file="ArrayExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using JetBrains.Annotations;

    /// <summary>
    /// The array extensions class
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts an Array of arbitrary type to an array of type T. If a suitable converter cannot be found to do the conversion, a NotSupportedException is thrown.
        /// </summary>
        /// <typeparam name="T">The type of items in the output array.</typeparam>
        /// <param name="input">The input array.</param>
        /// <returns>The new array.</returns>
        /// <exception cref="NotSupportedException">A suitable converter cannot be found to do the conversion.</exception>
        [NotNull]
        [ItemCanBeNull]
        public static T[] ConvertTo<T>([NotNull][ItemCanBeNull]this Array input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<T[]>() != null);

            var result = new T[input.Length];
            var tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(input.GetValue(0).GetType()))
            {
                for (var i = 0; i < input.Length; i++)
                {
                    result[i] = (T)tc.ConvertFrom(input.GetValue(i));
                }
            }
            else
            {
                tc = System.ComponentModel.TypeDescriptor.GetConverter(input.GetValue(0).GetType());
                if (tc.CanConvertTo(typeof(T)))
                {
                    for (var i = 0; i < input.Length; i++)
                    {
                        result[i] = (T)tc.ConvertTo(input.GetValue(i), typeof(T));
                    }
                }
                else
                {
                    throw new NotSupportedException("A suitable converter cannot be found to do the conversion.");
                }
            }

            return result;
        }

        /// <summary>
        /// Converts an array of any type to <see cref="List{T}"/> passing a mapping
        /// delegate Func{object, T} that returns type T. If T is null, it will not be added
        /// to the collection. If the array is null, then a new instance of <see cref="List{T}"/> is returned.
        /// </summary>
        /// <typeparam name="T">The type of the items in the output list.</typeparam>
        /// <param name="items">The array of items.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <returns>The output list.</returns>
        [ItemNotNull]
        [NotNull]
        public static List<T> ToList<T>([ItemCanBeNull][CanBeNull] this Array items, [CanBeNull] Func<object, T> mapFunction)
        {
            Contract.Ensures(Contract.Result<List<T>>() != null);

            if (items == null || mapFunction == null)
            {
                return new List<T>();
            }

            var coll = new List<T>();
            for (var i = 0; i < items.Length; i++)
            {
                var handler = mapFunction;
                if (handler != null)
                {
                    var val = handler(items.GetValue(i));
                    if (!EqualityComparer<T>.Default.Equals(val, default))
                    {
                        coll.Add(val);
                    }
                }
            }

            return coll;
        }
    }
}