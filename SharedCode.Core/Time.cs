// <copyright file="Time.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;

    /// <summary>
    /// Class Time. This class cannot be inherited.
    /// </summary>
    public sealed class Time
    {
        /// <summary>
        /// Computes the time zone variance.
        /// </summary>
        /// <returns>The time zone variance.</returns>
        public static int ComputeTimeZoneVariance()
        {
            var currentTime = DateTimeOffset.UtcNow;
            var difference = currentTime - currentTime.ToUniversalTime();
            return Convert.ToInt32(difference.TotalMinutes);
        }
    }
}
