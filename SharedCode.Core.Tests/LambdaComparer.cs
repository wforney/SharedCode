// <copyright file="LambdaComparer.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The lambda comparer class.
    /// </summary>
    /// <typeparam name="T">The type of the objects being compared.</typeparam>
    /// <seealso cref="IComparer" />
    public class LambdaComparer<T> : IComparer, IComparer<T>
    {
        /// <summary>
        /// The compare function
        /// </summary>
        private readonly CompareFunc<T> compareFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T}"/> class.
        /// </summary>
        /// <param name="compareFunction">The compare function.</param>
        public LambdaComparer(CompareFunc<T> compareFunction)
        {
            this.compareFunction = compareFunction;
        }

        /// <summary>
        /// Compares the specified objects.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>The result.</returns>
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (!(x is T t1) || !(y is T t2))
            {
                return -1;
            }

            return this.compareFunction(t1, t2) ? 0 : 1;
        }

        /// <summary>
        /// Compares the specified objects.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>The result.</returns>
        public int Compare(T x, T y)
        {
            if (EqualityComparer<T>.Default.Equals(x, default) && EqualityComparer<T>.Default.Equals(y, default))
            {
                return 0;
            }

            if (!(x is T t1) || !(y is T t2))
            {
                return -1;
            }

            return this.compareFunction(t1, t2) ? 0 : 1;
        }
    }
}
