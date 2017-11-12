// <copyright file="RectAngles.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows
{
    using System;

    /// <summary>
    /// Angles of a rectangle.
    /// </summary>
    [Flags]
    public enum RectAngles
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The top left
        /// </summary>
        TopLeft = 1,

        /// <summary>
        /// The top right
        /// </summary>
        TopRight = 2,

        /// <summary>
        /// The bottom left
        /// </summary>
        BottomLeft = 4,

        /// <summary>
        /// The bottom right
        /// </summary>
        BottomRight = 8,

        /// <summary>
        /// All
        /// </summary>
        All = TopLeft | TopRight | BottomLeft | BottomRight
    }
}
