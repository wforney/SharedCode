// <copyright file="LambdaComparer.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using JetBrains.Annotations;

    /// <summary>
    ///     The lambda comparer class.
    /// </summary>
    /// <typeparam name="T">The type of the objects being compared.</typeparam>
    /// <seealso cref="IComparer" />
    public class LambdaComparer<T> : IComparer, IComparer<T>
    {
        /// <summary>
        ///     The compare function
        /// </summary>
        [NotNull] private readonly CompareFunc<T> compareFunction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LambdaComparer{T}" /> class.
        /// </summary>
        /// <param name="compareFunction">The compare function.</param>
        public LambdaComparer([NotNull] CompareFunc<T> compareFunction)
        {
            Contract.Requires(compareFunction != null);

            this.compareFunction = compareFunction;
        }

        /// <summary>
        ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        ///     A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in
        ///     the following table.Value Meaning Less than zero
        ///     <paramref name="x" /> is less than <paramref name="y" />. Zero
        ///     <paramref name="x" /> equals <paramref name="y" />. Greater than zero
        ///     <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare([CanBeNull] object x, [CanBeNull] object y)
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
        ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        ///     A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in
        ///     the following table.Value Meaning Less than zero
        ///     <paramref name="x" /> is less than <paramref name="y" />.Zero
        ///     <paramref name="x" /> equals <paramref name="y" />.Greater than zero
        ///     <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare([CanBeNull]T x, [CanBeNull]T y)
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

        /// <summary>
        /// Objects the invariant.
        /// </summary>
        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "Required for code contracts.")]
        [Conditional("CONTRACTS_FULL")]
#pragma warning disable CC0091 // Use static method
        private void ObjectInvariant()
#pragma warning restore CC0091 // Use static method
        {
            Contract.Invariant(this.compareFunction != null);
        }
    }
}