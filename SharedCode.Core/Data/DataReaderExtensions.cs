// <copyright file="DataReaderExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    /// The data reader extensions class
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Returns an enumerable from this data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>An enumerable of data records.</returns>
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<IDataRecord> AsEnumerable([CanBeNull] this IDataReader dataReader)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IDataRecord>>() != null);

            if (dataReader == null)
            {
                yield break;
            }

            while (dataReader.Read())
            {
                yield return dataReader;
            }
        }

        /// <summary>
        /// Determines whether The specified column value is null.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>A value indicating whether or not the specified column is null.</returns>
        public static bool IsDBNull([CanBeNull] this IDataReader dataReader, [NotNull] string columnName)
            => dataReader?.IsDBNull(dataReader.GetOrdinal(columnName)) ?? false;

        /// <summary>
        /// Returns a list of delimited lines from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="separator">The value separator.</param>
        /// <param name="includeHeaderAsFirstRow">if set to <c>true</c> include header as first row.</param>
        /// <returns>A list of delimited lines.</returns>
        [NotNull]
        [ItemNotNull]
        public static List<string> ToDelimited(
            [NotNull] this IDataReader dataReader,
            [CanBeNull] string separator,
            bool includeHeaderAsFirstRow)
        {
            Contract.Requires(dataReader != null);
            Contract.Ensures(Contract.Result<List<string>>() != null);

            var output = new List<string>();
            StringBuilder sb = null;

            if (includeHeaderAsFirstRow)
            {
                sb = new StringBuilder();
                for (var index = 0; index < dataReader.FieldCount; index++)
                {
                    if (dataReader.GetName(index) != null)
                    {
                        sb.Append(dataReader.GetName(index));
                    }

                    if (index < dataReader.FieldCount - 1)
                    {
                        sb.Append(separator);
                    }
                }

                output.Add(sb.ToString());
            }

            while (dataReader.Read())
            {
                sb = new StringBuilder();
                for (var index = 0; index < dataReader.FieldCount - 1; index++)
                {
                    if (!dataReader.IsDBNull(index))
                    {
                        var value = dataReader.GetValue(index).ToString();
                        if (dataReader.GetFieldType(index) == typeof(string))
                        {
                            // If double quotes are used in value, ensure each are replaced but 2.
                            if (value.IndexOf("\"", StringComparison.Ordinal) >= 0)
                            {
                                value = value.Replace("\"", "\"\"");
                            }

                            // If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(separator, StringComparison.Ordinal) >= 0)
                            {
                                value = $"\"{value}\"";
                            }
                        }

                        sb.Append(value);
                    }

                    if (index < dataReader.FieldCount - 1)
                    {
                        sb.Append(separator);
                    }
                }

                if (!dataReader.IsDBNull(dataReader.FieldCount - 1))
                {
                    sb.Append(dataReader.GetValue(dataReader.FieldCount - 1).ToString().Replace(separator, " "));
                }

                output.Add(sb.ToString());
            }

            dataReader.Close();
            sb = null;
            return output;
        }

        /// <summary>
        /// Returns the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The value.</returns>
        [CanBeNull]
        public static T ValueOrDefault<T>([CanBeNull] this IDataReader dataReader, [NotNull] string columnName)
        {
            Contract.Requires(columnName != null);

            var value = dataReader?[columnName];
            return DBNull.Value == value ? default : (T)value;
        }

        /// <summary>
        /// Returns the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>The value.</returns>
        [CanBeNull]
        public static T ValueOrDefault<T>([CanBeNull] this IDataReader dataReader, int columnIndex)
        {
            return (dataReader?.IsDBNull(columnIndex) ?? true) || dataReader?.FieldCount <= columnIndex || columnIndex < 0
                ? default
                : (T)dataReader?[columnIndex];
        }
    }
}