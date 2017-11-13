// <copyright file="StoryboardExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Class StoryboardExtensions.
    /// </summary>
    public static class StoryboardExtensions
    {
        /// <summary>
        /// Begins the asynchronous.
        /// </summary>
        /// <param name="storyboard">The animation storyboard.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public static Task BeginAsync(this Storyboard storyboard)
        {
            var tcs = new TaskCompletionSource<bool>();
            if (storyboard == null)
            {
                tcs.SetException(new ArgumentNullException());
            }
            else
            {
                EventHandler onComplete = null;
                onComplete =
                    (s, e) =>
                    {
                        storyboard.Completed -= onComplete;
                        tcs.SetResult(result: true);
                    };
                storyboard.Completed += onComplete;
                storyboard.Begin();
            }

            return tcs.Task;
        }
    }
}
