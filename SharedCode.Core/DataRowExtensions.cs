// <copyright file="DataRowExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System.Data;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    /// <summary>
    ///     The data row extensions class
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        ///     Creates a cloned and detached copy of a DataRow instance
        /// </summary>
        /// <typeparam name="T">The type of the DataRow if strongly typed</typeparam>
        /// <param name="dataRow">The data row.</param>
        /// <param name="parentTable">The parent table.</param>
        /// <returns>An instance of the new DataRow</returns>
        [NotNull]
        public static T Clone<T>([NotNull] this DataRow dataRow, [NotNull] DataTable parentTable)
            where T : DataRow
        {
            Contract.Requires(dataRow != null);
            Contract.Requires(parentTable != null);
            Contract.Ensures(Contract.Result<T>() != null);

            var result = (T)parentTable.NewRow();
            result.ItemArray = dataRow.ItemArray;
            return result;
        }
    }
}