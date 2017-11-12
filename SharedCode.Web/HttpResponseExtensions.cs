// <copyright file="HttpResponseExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Web
{
    using System.Web;

    /// <summary>
    /// The HTTP response extensions class
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Forces the download of the specified file.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="fullPathToFile">The full path to file.</param>
        /// <param name="outputFileName">The name of the output file.</param>
        public static void ForceDownload(this HttpResponse response, string fullPathToFile, string outputFileName)
        {
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=" + outputFileName);
            response.WriteFile(fullPathToFile);
            response.ContentType = "";
            response.End();
        }
    }
}
