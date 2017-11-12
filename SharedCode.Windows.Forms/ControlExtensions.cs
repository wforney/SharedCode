// <copyright file="ControlExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.Forms
{
    using System.Windows.Forms;

    /// <summary>
    /// The Windows Forms control extensions class
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// The invoke handler delegate.
        /// </summary>
        public delegate void InvokeHandler();

        /// <summary>
        /// Safely invokes the specified handler.
        /// </summary>
        /// <param name="control">The Windows forms control.</param>
        /// <param name="handler">The handler to invoke.</param>
        public static void SafeInvoke(this Control control, InvokeHandler handler)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(handler);
            }
            else
            {
                handler?.Invoke();
            }
        }
    }
}
