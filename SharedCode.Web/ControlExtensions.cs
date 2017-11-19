// <copyright file="ControlExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Web
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.UI;

    using JetBrains.Annotations;

    /// <summary>
    ///     The control extensions class
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        ///     Finds the control.
        /// </summary>
        /// <typeparam name="T">The type of the control to find.</typeparam>
        /// <param name="startingControl">The starting control.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The control.</returns>
        [CanBeNull]
        public static T FindControl<T>([NotNull] this Control startingControl, [NotNull] string id) where T : Control
        {
            Contract.Requires(startingControl != null);
            Contract.Requires(id != null);

            var foundControl = default(T);

            foreach (Control c in startingControl.Controls)
            {
                if (c is T
                    && string.Equals(
                        id,
                        c.ID,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    foundControl = c as T;
                    break;
                }

                foundControl = c.FindControl<T>(id);
                if (foundControl != null)
                {
                    break;
                }
            }

            return foundControl;
        }
    }
}