// <copyright file="AssertExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

#pragma warning disable CC0057 // Unused parameters
#pragma warning disable RECS0154 // Parameter is never used
#pragma warning disable RCS1175 // Unused this parameter.

namespace SharedCode.Core.Tests
{
    using System.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The assert extensions class.
    /// </summary>
    public static class AssertExtensions
    {
        /// <summary>
        /// Asserts that the expected and actual values are equal using the specified comparer.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="assert">The assert class.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer class.</param>
        public static void AreEqual<T>(this Assert assert, T expected, T actual, IComparer comparer)
            => CollectionAssert.AreEqual(
                new[] { expected },
                new[] { actual },
                comparer,
                $"\nExpected: <{expected}>.\nActual: <{actual}>.");

        /// <summary>
        /// Asserts that the expected and actual values are equal using the specified comparer.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="assert">The assert class.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="compareFunction">The compare function.</param>
        public static void AreEqual<T>(this Assert assert, T expected, T actual, CompareFunc<T> compareFunction)
            => CollectionAssert.AreEqual(
                new[] { expected },
                new[] { actual },
                new LambdaComparer<T>(compareFunction),
                $"\nExpected: <{expected}>.\nActual: <{actual}>.");
    }
}

#pragma warning restore RCS1175 // Unused this parameter.
#pragma warning restore RECS0154 // Parameter is never used
#pragma warning restore CC0057 // Unused parameters
