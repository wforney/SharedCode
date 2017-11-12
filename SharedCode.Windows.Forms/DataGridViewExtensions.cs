// <copyright file="DataGridViewExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.Forms
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    /// <summary>
    /// The data grid view extensions class
    /// </summary>
    public static class DataGridViewExtensions
    {
        /// <summary>
        /// Loads columns information from the specified XML file
        /// </summary>
        /// <param name="dgv">DataGridView control instance</param>
        /// <param name="fileName">XML configuration file</param>
        public static void LoadConfiguration(this DataGridView dgv, string fileName)
        {
            using (var streamReader = new StreamReader(fileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ColumnInfo>));
                foreach (var column in (List<ColumnInfo>)xmlSerializer.Deserialize(streamReader))
                {
                    dgv.Columns[column.Name].DisplayIndex = column.DisplayIndex;
                    dgv.Columns[column.Name].Width = column.Width;
                    dgv.Columns[column.Name].Visible = column.Visible;
                }
            }
        }

        /// <summary>
        /// Saves columns information to the specified XML file
        /// </summary>
        /// <param name="dgv">DataGridView control instance</param>
        /// <param name="fileName">XML configuration file</param>
        public static void SaveConfiguration(this DataGridView dgv, string fileName)
        {
            var columns = new List<ColumnInfo>();
            for (var i = 0; i < dgv.Columns.Count; i++)
            {
                var column = new ColumnInfo
                {
                    Name = dgv.Columns[i].Name,
                    DisplayIndex = dgv.Columns[i].DisplayIndex,
                    Width = dgv.Columns[i].Width,
                    Visible = dgv.Columns[i].Visible
                };

                columns.Add(column);
            }

            using (var streamWriter = new StreamWriter(fileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ColumnInfo>));
                xmlSerializer.Serialize(streamWriter, columns);
            }
        }
    }
}
