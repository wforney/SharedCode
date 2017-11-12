// <copyright file="DirectoryInfoExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System.IO;

    /// <summary>
    /// The directory information extensions class
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Recursively create directory
        /// </summary>
        /// <param name="dirInfo">Folder path to create.</param>
        public static void CreateDirectory(this DirectoryInfo dirInfo)
        {
            if (dirInfo.Parent != null)
            {
                CreateDirectory(dirInfo.Parent);
            }

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
    }
}
