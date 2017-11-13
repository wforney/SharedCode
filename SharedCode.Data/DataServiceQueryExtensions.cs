// <copyright file="DataServiceQueryExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data
{
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// The data service query extensions class
    /// </summary>
    public static class DataServiceQueryExtensions
    {
        /// <summary>
        /// Executes the data service query asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items in the resulting enumerable.</typeparam>
        /// <param name="query">The data service query.</param>
        /// <returns>A task returning an enumerable.</returns>
        [ItemCanBeNull]
        [NotNull]
        public static Task<IEnumerable<T>> QueryAsync<T>([ItemCanBeNull][NotNull] this DataServiceQuery<T> query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<T>>>() != null);

            return Task<IEnumerable<T>>.Factory.FromAsync(query.BeginExecute, query.EndExecute, null);
        }
    }
}
