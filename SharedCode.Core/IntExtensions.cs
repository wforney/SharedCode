// <copyright file="IntExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;

    /// <summary>
    /// The integer extensions class
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Determines whether the specified number is prime.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns><c>true</c> if the specified number is prime; otherwise, <c>false</c>.</returns>
        public static bool IsPrime(this int number)
        {
            if ((number % 2) == 0)
            {
                return number == 2;
            }

            var sqrt = (int)Math.Sqrt(number);
            for (var t = 3; t <= sqrt; t += 2)
            {
                if (number % t == 0)
                {
                    return false;
                }
            }

            return number != 1;
        }
    }
}
