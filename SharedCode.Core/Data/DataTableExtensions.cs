// <copyright file="DataTableExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Data
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Text;
    using System.Xml.Linq;

    using JetBrains.Annotations;

    /// <summary>
    ///     The data table extensions class.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        ///     Converts a data table to a delimited string.
        /// </summary>
        /// <param name="table">        The data table.</param>
        /// <param name="delimiter">    The column delimiter.</param>
        /// <param name="includeHeader">if set to <c>true</c> [include header].</param>
        [NotNull]
        public static string ToDelimitedString([NotNull] this DataTable table, [CanBeNull] string delimiter, bool includeHeader)
        {
            Contract.Requires(table != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var result = new StringBuilder();

            result.Append(string.Empty);

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
                        // Double up all embedded double quotes To keep things simple, always delimit
                        // with double-quotes so we don't have to determine in which cases they're
                        // necessary and which cases they're not.
                        result.Append("\"").Append(item.ToString().Replace("\"", "\"\"")).Append("\"").Append(delimiter);
                    }
                }

                result.Remove(result.Length - 1, 1);
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts the data table to XML.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="rootName">Name of the XML root node.</param>
        /// <returns>An XML document.</returns>
        [NotNull]
        public static XDocument ToXml([NotNull] this DataTable dataTable, [NotNull] string rootName)
        {
            Contract.Requires(dataTable != null);
            Contract.Requires(rootName != null);
            Contract.Ensures(Contract.Result<XDocument>() != null);

            var xdoc = new XDocument
            {
                Declaration = new XDeclaration("1.0", "utf-8", "")
            };

            xdoc.Add(new XElement(rootName));

            foreach (DataRow row in dataTable.Rows)
            {
                var element = new XElement(dataTable.TableName);
                foreach (DataColumn col in dataTable.Columns)
                {
                    element.Add(new XElement(col.ColumnName, row[col].ToString().Trim(' ')));
                }

                xdoc.Root?.Add(element);
            }

            return xdoc;
        }
    }
}