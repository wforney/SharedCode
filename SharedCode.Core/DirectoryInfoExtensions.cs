﻿// <copyright file="DirectoryInfoExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System.Diagnostics.Contracts;
    using System.IO;

    using JetBrains.Annotations;

    /// <summary>
    /// The directory information extensions class
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Recursively create directory
        /// </summary>
        /// <param name="dirInfo">Folder path to create.</param>
        public static void CreateDirectory([NotNull] this DirectoryInfo dirInfo)
        {
            Contract.Requires(dirInfo != null);

            if (dirInfo.Parent != null)
            {
                DirectoryInfoExtensions.CreateDirectory(dirInfo.Parent);
            }

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
    }
}
