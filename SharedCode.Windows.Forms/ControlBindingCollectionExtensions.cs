// <copyright file="ControlBindingCollectionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.Forms
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Windows.Forms;

    using JetBrains.Annotations;

    /// <summary>
    ///     The control binding collection extensions class.
    /// </summary>
    public static class ControlBindingCollectionExtensions
    {
        /// <summary>
        ///     Adds databinding to a control.
        /// </summary>
        /// <typeparam name="T">The type of the data source.</typeparam>
        /// <param name="bindingCollection">The binding collection.</param>
        /// <param name="property">The property name.</param>
        /// <param name="datasource">The data source.</param>
        /// <param name="expression">The binding expression.</param>
        /// <returns>A <see cref="Binding" />.</returns>
        [NotNull]
        public static Binding Add<T>(
            [NotNull] [ItemCanBeNull] this ControlBindingsCollection bindingCollection,
            [NotNull] string property,
            [NotNull] object datasource,
            [NotNull] Expression<Func<T, object>> expression)
        {
            Contract.Requires(bindingCollection != null);
            Contract.Requires(property != null);
            Contract.Requires(datasource != null);
            Contract.Requires(expression != null);
            Contract.Ensures(Contract.Result<Binding>() != null);

            string relatedNameChain;
            switch (expression.Body)
            {
                case UnaryExpression body:
                    relatedNameChain = (body.Operand as MemberExpression)?.ToString() ?? string.Empty;
                    break;

                case MemberExpression body:
                    relatedNameChain = body.ToString();
                    break;

                default:
                    relatedNameChain = string.Empty;
                    break;
            }

            var skippedName = string.Join(".", relatedNameChain.Split('.').Skip(1).ToArray());
            return bindingCollection.Add(property, datasource, skippedName);
        }
    }
}