// <copyright file="StreamExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core.IO
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    ///     The stream extensions class
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the content of the stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <returns>Returns a string with the content of the input stream.</returns>
        [CanBeNull]
        public static string ReadToEnd([NotNull] this Stream stream)
        {
            Contract.Requires(stream != null);

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads the content of the stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <returns>Returns a string with the content of the input stream.</returns>
        [NotNull]
        [ItemCanBeNull]
        public static async Task<string> ReadToEndAsync([NotNull] this Stream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<Task<string>>() != null);

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        /// <summary>
        ///     Converts the input stream to a byte array.
        /// </summary>
        /// <param name="input">This input stream.</param>
        /// <returns>The byte array.</returns>
        [NotNull]
        public static byte[] ToByteArray([NotNull] this Stream input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            if (input is MemoryStream stream)
            {
                return stream.ToArray();
            }

            using (var ms = new MemoryStream())
            {
                if (input.CanSeek)
                {
                    input.Seek(0, SeekOrigin.Begin);
                }

                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Converts the input stream to a byte array.
        /// </summary>
        /// <param name="input">This input stream.</param>
        /// <returns>The byte array.</returns>
        [NotNull]
        [ItemCanBeNull]
        public static async Task<byte[]> ToByteArrayAsync([NotNull] this Stream input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<Task<byte[]>>() != null);

            if (input is MemoryStream stream)
            {
                return stream.ToArray();
            }

            using (var ms = new MemoryStream())
            {
                if (input.CanSeek)
                {
                    input.Seek(0, SeekOrigin.Begin);
                }

                await input.CopyToAsync(ms).ConfigureAwait(continueOnCapturedContext: false);
                return ms.ToArray();
            }
        }
    }
}