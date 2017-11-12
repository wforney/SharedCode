// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;
    using System.Reflection;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The string extensions class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="key">Decryptionkey.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            string result = null;

            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            using (var rsa = new RSACryptoServiceProvider(new CspParameters { KeyContainerName = key }) { PersistKeyInCsp = true })
            {
                var decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                var decryptByteArray = Array.ConvertAll(decryptArray, s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber)));

                var bytes = rsa.Decrypt(decryptByteArray, fOAEP: true);

                result = Encoding.UTF8.GetString(bytes);

                return result;
            }
        }

        /// <summary>
        /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryptionkey.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            using (var rsa = new RSACryptoServiceProvider(new CspParameters { KeyContainerName = key }) { PersistKeyInCsp = true })
            {
                var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), fOAEP: true);

                return BitConverter.ToString(bytes);
            }
        }

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

        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var output = string.Empty;
            var index = 0;
            var builder = new StringBuilder();
            builder.Append(output);
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        builder.Append(input[index]);
                        index++;
                    }
                }
                else
                {
                    builder.Append(m);
                }
            }

            output = builder.ToString();

            return output;
        }

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
        /// Determines whether the specified input string is a date.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the specified input string is a date; otherwise, <c>false</c>.</returns>
        public static bool IsDate(this string input) => !string.IsNullOrEmpty(input) && DateTime.TryParse(input, out var dt);

        /// <summary>
        /// Determines whether the specified string is numeric.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the specified string is numeric; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this string input) => long.TryParse(input, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);

        /// <summary>
        /// Determines whether the input string is a valid email address.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the input string is a valid email address; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmailAddress(this string input)
        {
            try
            {
                var addr = new MailAddress(input);
                return true;
#pragma warning disable GCop138 // When you catch an exception you should throw exception or at least log error
            }
            catch (Exception)
            {
#pragma warning restore GCop138 // When you catch an exception you should throw exception or at least log error
                return false;
            }
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > Math.Max(length, 0) ? value.Substring(0, Math.Max(length, 0)) : value;
        }

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
        /// Parses the specified value.
        /// </summary>
        /// <typeparam name="T">The type to parse the string into.</typeparam>
        /// <param name="value">The string value.</param>
        /// <returns>The parsed output.</returns>
        public static T Parse<T>(this string value)
        {
            // Get default value for type so if string is empty then we can return default value.
            var result = default(T);
            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            // We are not going to handle exceptions here. If you need SafeParse then you should create another method specially for that.
            var tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(value);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
            => value != null && value.Length > length ? value.Substring(value.Length - length) : value;

        /// <summary>
        /// Strips the HTML from the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The output string.</returns>
        public static string StripHtml(this string input)
        {
            // Will this simple expression replace all tags???
            var tagsExpression = new Regex("</?.+?>");
            return tagsExpression.Replace(input, " ");
        }

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
        /// Converts a string into a "SecureString"
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The secure string.</returns>
        public static SecureString ToSecureString(this string input)
        {
            var secureString = new SecureString();
            foreach (var character in input)
            {
                secureString.AppendChar(character);
            }

            return secureString;
        }

        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">The string that will be truncated.</param>
        /// <param name="maxLength">The total length of characters to maintain before the truncate happens.</param>
        /// <param name="suffix">The suffix string.</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(this string text, int maxLength, string suffix = "...")
        {
            // replaces the truncated string to a ...
            var truncatedString = text;

            if (maxLength <= 0)
            {
                return truncatedString;
            }

            var strLength = maxLength - suffix.Length;

            if (strLength <= 0)
            {
                return truncatedString;
            }

            if (text == null || text.Length <= maxLength)
            {
                return truncatedString;
            }

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;

            return truncatedString;
        }

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
    }
}
