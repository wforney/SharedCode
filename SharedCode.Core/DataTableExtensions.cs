// <copyright file="DataTableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Data;
    using System.Text;

    /// <summary>
    /// The data table extensions class.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Converts a data table to a delimited string.
        /// </summary>
        /// <param name="table">The data table.</param>
        /// <param name="delimiter">The column delimiter.</param>
        /// <param name="includeHeader">if set to <c>true</c> [include header].</param>
        public static string ToDelimitedString(this DataTable table, string delimiter, bool includeHeader)
        {
            var result = new StringBuilder();

            if (includeHeader)
            {
                foreach (DataColumn column in table.Columns)
                {
                    result.Append(column.ColumnName);
                    result.Append(delimiter);
                }

                result.Remove(--result.Length, 0);
                result.Append(Environment.NewLine);
            }

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    if (item is DBNull)
                    {
                        result.Append(delimiter);
                    }
                    else
                    {
                        // Double up all embedded double quotes
                        // To keep things simple, always delimit with double-quotes
                        // so we don't have to determine in which cases they're necessary
                        // and which cases they're not.
                        result.Append("\"").Append(item.ToString().Replace("\"", "\"\"")).Append("\"").Append(delimiter);
                    }
                }

                result.Remove(result.Length - 1, 1);
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }
    }
}
