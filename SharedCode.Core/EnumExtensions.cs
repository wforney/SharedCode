// <copyright file="EnumExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>
using System;

namespace SharedCode.Core
{
    /// <summary>
    /// Class EnumExtensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether the specified match to is set.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="matchTo">The match to.</param>
        /// <returns><c>true</c> if the specified match to is set; otherwise, <c>false</c>.</returns>
        public static bool IsSet(this Enum input, Enum matchTo)
        {
            return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
        }
    }
}