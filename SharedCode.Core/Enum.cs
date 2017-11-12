// <copyright file="Enum.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The enumeration class.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    public static class Enum<T>
    {
        /// <summary>
        /// Converts an enumeration to a list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <exception cref="System.ArgumentException">T must be of type System.Enum</exception>
        public static List<T> EnumToList()
        {
            var enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be of type System.Enum");
            }

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}
