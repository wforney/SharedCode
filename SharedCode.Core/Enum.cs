// <copyright file="Enum.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The enumeration class.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    public static class Enum<T>
    {
        /// <summary>
        /// The string values
        /// </summary>
        private static readonly Hashtable StringValues = new Hashtable();

        /// <summary>
        /// Converts an enumeration to a list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <exception cref="System.ArgumentException">T must be of type System.Enum</exception>
        public static List<T> EnumToList()
        {
            var enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be of type System.Enum");
            }

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        /// <summary>
        /// Gets the values as a 'bindable' list datasource.
        /// </summary>
        /// <returns>IList for data binding</returns>
        public static IList GetListValues()
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            var values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (var fi in typeof(T).GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false);
                if (attrs.Any())
                {
                    values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(typeof(T), fi.Name), underlyingType), attrs.First().Value));
                }
            }

            return values;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(string stringValue) => Enum.Parse(typeof(T), stringValue) != null;

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(string stringValue, bool ignoreCase) => Enum.Parse(typeof(T), stringValue, ignoreCase) != null;

        /// <summary>
        /// Gets the underlying enum type for this instance.
        /// </summary>
        /// <value>The type of the enum.</value>
        public static Type EnumType => typeof(T);

        /// <summary>
        /// Gets the string value associated with the given enum value.
        /// </summary>
        /// <param name="valueName">Name of the enum value.</param>
        /// <returns>String Value</returns>
        public static string GetStringValue(string valueName)
        {
            Enum enumType;
            string stringValue = null;
            try
            {
                enumType = (Enum)Enum.Parse(typeof(T), valueName);
                stringValue = GetStringValue(enumType);
#pragma warning disable CC0004 // Catch block cannot be empty
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
#pragma warning disable GCop138 // When you catch an exception you should throw exception or at least log error
            }
            catch (Exception)
            {
                //Swallow!
#pragma warning restore GCop138 // When you catch an exception you should throw exception or at least log error
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore CC0004 // Catch block cannot be empty
            }

            return stringValue;
        }

        /// <summary>
        /// Gets the string values associated with the enum.
        /// </summary>
        /// <returns>String value array</returns>
        public static Array GetStringValues()
        {
            var values = new ArrayList();

            //Look for our string value associated with fields in this enum
            foreach (var fi in typeof(T).GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false);
                if (attrs.Any())
                {
                    values.Add(attrs.First().Value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The string value.</returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            var type = value.GetType();

            //Check first in our cached results...
            if (StringValues.ContainsKey(value))
            {
                output = (StringValues[value] as StringValueAttribute)?.Value;
            }
            else
            {
                //Look for our 'StringValueAttribute'
                //in the field's custom attributes
                var fi = type.GetField(value.ToString());
                var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    StringValues.Add(value, attrs[0]);
                    output = attrs[0].Value;
                }
            }

            return output;
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue) => Parse(type, stringValue, ignoreCase: false);

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
            {
                throw new ArgumentException($"Supplied type must be an Enum.  Type was {type}");
            }

            //Look for our string value associated with fields in this enum
            foreach (var fi in type.GetFields())
            {
                //Check for our custom attribute
                var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false);
                if (attrs.Any())
                {
                    enumStringValue = attrs.First().Value;
                }

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue) => Parse(enumType, stringValue) != null;

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase) => Parse(enumType, stringValue, ignoreCase) != null;

        /// <summary>
        /// Converts Enumeration type into a dictionary of names and values
        /// </summary>
        /// <returns>IDictionary&lt;System.String, System.Int32&gt;.</returns>
        /// <exception cref="ArgumentNullException">enumType</exception>
        /// <exception cref="InvalidCastException">object is not an Enumeration</exception>
        public static IDictionary<string, int> ToDictionary()
        {
            if (typeof(T) == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException("object is not an Enumeration");
            }

            var names = Enum.GetNames(typeof(T));
            var values = Enum.GetValues(typeof(T));

            return (from i in Enumerable.Range(0, names.Length)
                    select new { Key = names[i], Value = (int)values.GetValue(i) })
                        .ToDictionary(k => k.Key, k => k.Value);
        }
    }
}
