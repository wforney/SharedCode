// <copyright file="DataReaderExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

using JetBrains.Annotations;
using System.Diagnostics.Contracts;

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    /// <summary>
    /// Class DataReaderExtensions.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Returns a list of delimited lines from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="separator">The value separator.</param>
        /// <param name="includeHeaderAsFirstRow">if set to <c>true</c> include header as first row.</param>
        /// <returns>A list of delimited lines.</returns>
        [NotNull]
        [ItemNotNull]
        public static List<string> ToDelimited([NotNull] this IDataReader dataReader, [CanBeNull] string separator, bool includeHeaderAsFirstRow)
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
                            //If double quotes are used in value, ensure each are replaced but 2.
                            if (value.IndexOf("\"", StringComparison.Ordinal) >= 0)
                            {
                                value = value.Replace("\"", "\"\"");
                            }

                            //If separtor are is in value, ensure it is put in double quotes.
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
    }
}