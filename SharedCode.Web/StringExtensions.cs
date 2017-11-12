// <copyright file="StringExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Web
{
    using System.Collections.Specialized;
    using System.Web;

    /// <summary>
    /// The string extensions class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Decodes the specified string into an HTML string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The decoded HTML string.</returns>
        public static string HtmlDecode(this string input) => HttpUtility.HtmlDecode(input);

        /// <summary>
        /// HTML encodes the specified input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The HTML encoded string.</returns>
        public static string HtmlEncode(this string input) => HttpUtility.HtmlEncode(input);

        /// <summary>
        /// Parses the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>A <see cref="NameValueCollection"/>.</returns>
        public static NameValueCollection ParseQueryString(this string queryString) => HttpUtility.ParseQueryString(queryString);

        /// <summary>
        /// Decodes the specified URL encoded string.
        /// </summary>
        /// <param name="url">The URL encoded string.</param>
        /// <returns>The decoded URL string.</returns>
        public static string UrlDecode(this string url) => HttpUtility.UrlDecode(url);

        /// <summary>
        /// URL encodes the specified string.
        /// </summary>
        /// <param name="url">The URL string.</param>
        /// <returns>The URL encoded string.</returns>
        public static string UrlEncode(this string url) => HttpUtility.UrlEncode(url);

        /// <summary>
        /// URL encodes the specified PATH string.
        /// </summary>
        /// <param name="url">The URL path string.</param>
        /// <returns>The encoded URL path string.</returns>
        public static string UrlPathEncode(this string url) => HttpUtility.UrlPathEncode(url);
    }
}
