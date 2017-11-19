// <copyright file="ObservableCollectionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    /// <summary>
    ///     The observable collection extensions class.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        ///     Adds the specified enumerable to the observable collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="observableCollection">The observable collection.</param>
        /// <param name="enumerable">The enumerable to add.</param>
        /// <returns>The observable collection.</returns>
        /// <exception cref="ArgumentNullException">collection</exception>
        [NotNull]
        [ItemCanBeNull]
        public static ObservableCollection<T> AddRange<T>(
            [NotNull] [ItemCanBeNull] this ObservableCollection<T> observableCollection,
            [NotNull] [ItemCanBeNull] IEnumerable<T> enumerable)
        {
            Contract.Requires(observableCollection != null);
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<ObservableCollection<T>>() != null);

            if (observableCollection == null)
            {
                throw new ArgumentNullException(nameof(observableCollection));
            }

            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            foreach (var item in enumerable)
            {
                observableCollection.Add(item);
            }

            return observableCollection;
        }
    }
}