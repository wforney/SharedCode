// <copyright file="ListExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Office
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Office.Interop.Excel;
    using System.Drawing;

    /// <summary>
    /// The list extensions class.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Writes list data to Excel.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="list">The input list.</param>
        /// <param name="pathToSave">Path to save file.</param>
        /// <exception cref="Exception">
        /// Invalid file path or no data to export.
        /// </exception>
        public static void ToExcel<T>(this List<T> list, string pathToSave)
        {
            if (string.IsNullOrEmpty(pathToSave))
            {
                throw new Exception("Invalid file path.");
            }

            if (pathToSave.IndexOf("", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new Exception("Invalid file path.");
            }

            if (list == null)
            {
                throw new Exception("No data to export.");
            }

            // Optional argument variable
            object optionalValue = Missing.Value;

            const string strHeaderStart = "A2";
            const string strDataStart = "A3";

            // Initialize Excel
            var excelApp = new Application();
            var books = excelApp.Workbooks;
            var book = books.Add(optionalValue);
            var sheets = book.Worksheets;
            var sheet = (_Worksheet)(sheets.get_Item(1));

            // Create header
            CreateHeader<T>(out var range, out var font, optionalValue, strHeaderStart, sheet, out var objHeaders);

            // Write data to cells
            range = WriteData(list, optionalValue, strHeaderStart, strDataStart, sheet, objHeaders);

            // Saving data and Opening Excel file.
            if (!string.IsNullOrEmpty(pathToSave))
            {
                book.SaveAs(pathToSave);
            }

            excelApp.Visible = true;

            // Release objects
            try
            {
                if (sheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                }

                sheet = null;

                if (sheets != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                }

                sheets = null;

                if (book != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                }

                book = null;

                if (books != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
                }

                books = null;

                if (excelApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                excelApp = null;
#pragma warning disable GCop138 // When you catch an exception you should throw exception or at least log error
            }
            catch (Exception)
            {
                sheet = null;
                sheets = null;
                book = null;
                books = null;
                excelApp = null;
#pragma warning restore GCop138 // When you catch an exception you should throw exception or at least log error
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
        /// <param name="source">The enumerable.</param>
        /// <param name="optionalValue">The optional value.</param>
        /// <param name="strHeaderStart">The string header start.</param>
        /// <param name="strDataStart">The string data start.</param>
        /// <param name="sheet">The worksheet.</param>
        /// <param name="objHeaders">The object headers.</param>
        /// <returns>The Excel range.</returns>
        private static Range WriteData<T>(IList<T> source, object optionalValue, string strHeaderStart, string strDataStart, _Worksheet sheet, Dictionary<string, string> objHeaders)
        {
            Range range;
            var count = source.Count;
            var objData = new object[count, objHeaders.Count];

            for (var j = 0; j < count; j++)
            {
                var item = source[j];
                var col = 0;
                foreach (var entry in objHeaders)
                {
                    var row = typeof(T).InvokeMember(entry.Key, BindingFlags.GetProperty, null, item, null);
                    objData[j, col++] = (row == null) ? "" : row.ToString();
                }
            }

            range = sheet.get_Range(strDataStart, optionalValue);
            range = range.get_Resize(count, objHeaders.Count);

            range.set_Value(optionalValue, objData);
            range.BorderAround(Type.Missing, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            range = sheet.get_Range(strHeaderStart, optionalValue);
            range = range.get_Resize(count + 1, objHeaders.Count);
            range.Columns.AutoFit();
            return range;
        }

        /// <summary>
        /// Creates the header.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="range">The Excel range.</param>
        /// <param name="font">The Excel font.</param>
        /// <param name="optionalValue">The optional value.</param>
        /// <param name="strHeaderStart">The string header start.</param>
        /// <param name="sheet">The worksheet.</param>
        /// <param name="objHeaders">The object headers.</param>
#pragma warning disable GCop119 // Don’t use {0} parameters in method definition. To return several objects, define a class or struct for your method return type.
        private static void CreateHeader<T>(out Range range, out Font font, object optionalValue, string strHeaderStart, _Worksheet sheet, out Dictionary<string, string> objHeaders)
#pragma warning restore GCop119 // Don’t use {0} parameters in method definition. To return several objects, define a class or struct for your method return type.
        {
            objHeaders = new Dictionary<string, string>();
            foreach (var property in typeof(T).GetProperties())
            {
                var attribute = property.GetCustomAttributes<DisplayNameAttribute>(inherit: false).FirstOrDefault();
                objHeaders.Add(property.Name, attribute == null ? property.Name : attribute.DisplayName);
            }

            range = sheet.get_Range(strHeaderStart, optionalValue);
            range = range.get_Resize(1, objHeaders.Count);

            range.set_Value(optionalValue, objHeaders.Values.ToArray());
            range.BorderAround(Type.Missing, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            font = range.Font;
            font.Bold = true;
            range.Interior.Color = Color.LightGray.ToArgb();
        }
    }
}
