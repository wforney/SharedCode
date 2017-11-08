// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The string extensions class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the enumeration value description.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The enumeration value description.</returns>
        public static string GetEnumDescription<T>(string value)
        {
            var type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (name == null)
            {
                return string.Empty;
            }

            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes<DescriptionAttribute>(inherit: false);
            return customAttribute.Any() ? customAttribute.First().Description : name;
        }

        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (var otherValue in stringValues)
            {
                if (string.Compare(value, otherValue) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified string is numeric.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the specified string is numeric; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this string input) => float.TryParse(input, out var output);

        /// <summary>
        /// Returns the string with the specified value or null if the value is empty.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The result.</returns>
        public static string NullIfEmpty(this string value) => string.IsNullOrEmpty(value) ? default : value;

        /// <summary>
        /// Returns the string with the specified value or null if the value is white space.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The result.</returns>
        public static string NullIfWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value) ? default : value;

        /// <summary>
        /// Upper case the first letter in the string.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The upper cased string.</returns>
        public static string UppercaseFirstLetter(this string value)
        {
            if (value.Length > 0)
            {
                var array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }

            return value;
        }

        /// <summary>
        /// Returns the string with the specified value or an empty string if value is null.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The result.</returns>
        public static string ValueOrEmpty(this string value) => string.IsNullOrEmpty(value) ? string.Empty : value;

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
            => (T)Enum.Parse(typeof(T), value, ignoreCase: true);

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
            => value != null && value.Length > length ? value.Substring(value.Length - length) : value;

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
            => value != null && value.Length > length ? value.Substring(0, length) : value;

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>A copy of format in which the first format item has been replaced by the System.String equivalent of arg0</returns>
        public static string Format(this string value, object arg0)
            => string.Format(value, arg0);

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the System.String equivalent of the corresponding instances of System.Object in args.</returns>
        public static string Format(this string value, params object[] args)
            => string.Format(value, args);
    }
}
