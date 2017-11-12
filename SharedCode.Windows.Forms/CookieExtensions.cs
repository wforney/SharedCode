// <copyright file="CookieExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.Forms
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The cookie extensions class
    /// </summary>
    public static class CookieExtensions
    {
        /// <summary>
        /// Deletes a specified cookie by setting its value to empty and expiration to -1 days
        /// </summary>
        /// <param name="doc">The HtmDocument to extend</param>
        /// <param name="key">the cookie key to delete</param>
        public static void DeleteCookie(this HtmlDocument doc, string key)
        {
            var oldCookie = doc.Cookie;
            var expiration = DateTime.UtcNow - TimeSpan.FromDays(1);
            doc.Cookie = $"{key}=;expires={expiration.ToString("R")}";
        }

        /// <summary>
        /// Retrieves an existing cookie
        /// </summary>
        /// <param name="doc">The HtmDocument to extend</param>
        /// <param name="key">cookie key</param>
        /// <returns>null if the cookie does not exist, otherwise the cookie value</returns>
        public static string GetCookie(this HtmlDocument doc, string key)
        {
            var cookies = doc.Cookie.Split(';');
            key += '=';
            foreach (var cookie in cookies)
            {
                var cookieStr = cookie.Trim();
                if (!cookieStr.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var vals = cookieStr.Split('=');

                if (vals.Length >= 2)
                {
                    return vals[1];
                }

                return string.Empty;
            }

            return null;
        }

        /// <summary>
        /// Sets a persistent cookie which expires after the given number of days
        /// </summary>
        /// <param name="doc">The HtmDocument to extend</param>
        /// <param name="key">the cookie key</param>
        /// <param name="value">the cookie value</param>
        /// <param name="days">The number of days before the cookie expires</param>
        public static void SetCookie(this HtmlDocument doc, string key, string value, int days) => SetCookie(doc, key, value, DateTime.UtcNow + TimeSpan.FromDays(days));

        /// <summary>
        /// Sets a persistent cookie with an expiration date
        /// </summary>
        /// <param name="doc">The HtmDocument to extend</param>
        /// <param name="key">the cookie key</param>
        /// <param name="value">the cookie value</param>
        /// <param name="expiration">The expiration date time.</param>
        public static void SetCookie(this HtmlDocument doc, string key, string value, DateTime expiration)
        {
            var oldCookie = doc.Cookie;
            doc.Cookie = $"{key}={value};expires={expiration.ToString("R")}";
        }
    }
}
