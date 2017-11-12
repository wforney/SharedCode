// <copyright file="ArrayExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The array extensions class
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts an array of any type to <see cref="List{T}"/> passing a mapping
        /// delegate Func{object, T} that returns type T. If T is null, it will not be added
        /// to the collection. If the array is null, then a new instance of <see cref="List{T}"/> is returned.
        /// </summary>
        /// <typeparam name="T">The type of the items in the output list.</typeparam>
        /// <param name="items">The array of items.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <returns>The output list.</returns>
        public static List<T> ToList<T>(this Array items, Func<object, T> mapFunction)
        {
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
