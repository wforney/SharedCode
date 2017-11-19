// <copyright file="Hasher.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The hasher class
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        /// Supported hash algorithms
        /// </summary>
        public enum EHashType
        {
            /// <summary>
            /// The HMAC
            /// </summary>
            HMAC,

            /// <summary>
            /// The HMAC MD5 //DevSkim: ignore DS126858
            /// </summary>
            HMACMD5, // DevSkim: ignore DS126858

            /// <summary>
            /// The HMAC SHA1 //DevSkim: ignore DS126858
            /// </summary>
            HMACSHA1, // DevSkim: ignore DS126858

            /// <summary>
            /// The HMAC SHA256
            /// </summary>
            HMACSHA256,

            /// <summary>
            /// The HMAC SHA384
            /// </summary>
            HMACSHA384,

            /// <summary>
            /// The HMAC SHA512
            /// </summary>
            HMACSHA512,

            /// <summary>
            /// The MAC Triple DES
            /// </summary>
            MACTripleDES,

            /// <summary>
            /// The MD5 //DevSkim: ignore DS126858
            /// </summary>
            MD5, // DevSkim: ignore DS126858

            /// <summary>
            /// The RIPEMD160 //DevSkim: ignore DS126858
            /// </summary>
            RIPEMD160, // DevSkim: ignore DS126858

            /// <summary>
            /// The SHA1 //DevSkim: ignore DS126858
            /// </summary>
            SHA1, // DevSkim: ignore DS126858

            /// <summary>
            /// The SHA256
            /// </summary>
            SHA256,

            /// <summary>
            /// The SHA384
            /// </summary>
            SHA384,

            /// <summary>
            /// The SHA512
            /// </summary>
            SHA512
        }

        /// <summary>
        /// Gets the hash bytes.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="hash">The hash type.</param>
        /// <returns>The hash byte array.</returns>
        private static byte[] GetHash(string input, EHashType hash)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);

            switch (hash)
            {
                case EHashType.HMAC:
                    return HMAC.Create().ComputeHash(inputBytes);

#pragma warning disable RECS0030 // Suggests using the class declaring a static function when calling it
                case EHashType.HMACMD5: // DevSkim: ignore DS126858
                    return HMACMD5.Create().ComputeHash(inputBytes); // DevSkim: ignore DS126858

                case EHashType.HMACSHA1: // DevSkim: ignore DS126858
                    return HMACSHA1.Create().ComputeHash(inputBytes); // DevSkim: ignore DS126858

                case EHashType.HMACSHA256:
                    return HMACSHA256.Create().ComputeHash(inputBytes);

                case EHashType.HMACSHA384:
                    return HMACSHA384.Create().ComputeHash(inputBytes);

                case EHashType.HMACSHA512:
                    return HMACSHA512.Create().ComputeHash(inputBytes);
#pragma warning restore RECS0030 // Suggests using the class declaring a static function when calling it

                case EHashType.MD5: // DevSkim: ignore DS126858
#pragma warning disable SG0006 // Weak hashing function
                    return MD5.Create().ComputeHash(inputBytes); // DevSkim: ignore DS126858
#pragma warning restore SG0006 // Weak hashing function

                case EHashType.SHA1: // DevSkim: ignore DS126858
#pragma warning disable SG0006 // Weak hashing function
                    return SHA1.Create().ComputeHash(inputBytes); // DevSkim: ignore DS126858
#pragma warning restore SG0006 // Weak hashing function

                case EHashType.SHA256:
                    return SHA256.Create().ComputeHash(inputBytes);

                case EHashType.SHA384:
                    return SHA384.Create().ComputeHash(inputBytes);

                case EHashType.SHA512:
                    return SHA512.Create().ComputeHash(inputBytes);

                default:
                    return inputBytes;
            }
        }

        /// <summary>
        /// Computes the hash of the string using a specified hash algorithm
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <param name="hashType">The hash algorithm to use</param>
        /// <returns>The resulting hash or an empty string on error</returns>
        public static string ComputeHash(this string input, EHashType hashType)
        {
            try
            {
                var hash = Hasher.GetHash(input, hashType);
                var stringBuilder = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }

                return stringBuilder.ToString();
#pragma warning disable GCop138 // When you catch an exception you should throw exception or at least log error
            }
            catch (Exception)
            {
#pragma warning restore GCop138 // When you catch an exception you should throw exception or at least log error
                return string.Empty;
            }
        }
    }
}