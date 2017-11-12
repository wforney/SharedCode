// <copyright file="ColumnInfo.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.Forms
{
    using System;

    /// <summary>
    /// Class ColumnInfo. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class ColumnInfo
    {
        /// <summary>
        /// Gets or sets the display index.
        /// </summary>
        /// <value>The display index.</value>
        public int DisplayIndex { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ColumnInfo"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
    }
}
