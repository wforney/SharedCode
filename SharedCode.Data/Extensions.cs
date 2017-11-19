// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;

    using Core;

    using JetBrains.Annotations;

    /// <summary>
    ///     The extensions class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Gets the maximum length of the property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns>The maximum length of the property.</returns>
        public static int GetMaxLength<TSource, TProperty>(
            [NotNull] this TSource source,
            [NotNull] Expression<Func<TSource, TProperty>> propertyLambda)
            where TSource : class, new()
            where TProperty : IComparable, ICloneable, IConvertible, IComparable<string>, IEnumerable<char>, IEnumerable
            , IEquatable<string>
        {
            Contract.Requires(source != null);
            Contract.Requires(propertyLambda != null);

            var propInfo = source.GetPropertyInfo(propertyLambda);
            var attrMaxLength =
                propInfo?.GetCustomAttributes(typeof(MaxLengthAttribute), inherit: false).FirstOrDefault() as MaxLengthAttribute;
            return attrMaxLength?.Length ?? 0;
        }
    }
}