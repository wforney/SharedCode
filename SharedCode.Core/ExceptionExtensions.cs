// <copyright file="ExceptionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    /// The exception extensions class
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Creates a log-string from the Exception.
        /// <para>The result includes the stacktrace, innerexception et cetera, separated by <seealso cref="Environment.NewLine" />.</para>
        /// </summary>
        /// <param name="ex">The exception to create the string from.</param>
        /// <param name="additionalMessage">Additional message to place at the top of the string, maybe be empty or null.</param>
        /// <returns>System.String.</returns>
        [NotNull]
        public static string ToLogString([CanBeNull] this Exception ex, [CanBeNull] string additionalMessage)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            var msg = new StringBuilder();
            msg.Append(string.Empty);

            if (!string.IsNullOrEmpty(additionalMessage))
            {
                msg.Append(additionalMessage);
                msg.Append(Environment.NewLine);
            }

            if (ex == null)
            {
                return msg.ToString();
            }

            try
            {
                var orgEx = ex;

                msg.Append("Exception:");
                msg.Append(Environment.NewLine);
                while (orgEx != null)
                {
                    msg.Append(orgEx.Message);
                    msg.Append(Environment.NewLine);
                    orgEx = orgEx.InnerException;
                }

                if (ex.Data != null)
                {
                    foreach (object i in ex.Data)
                    {
                        msg.Append("Data :");
                        msg.Append(i.ToString());
                        msg.Append(Environment.NewLine);
                    }
                }

                if (ex.StackTrace != null)
                {
                    msg.Append("StackTrace:");
                    msg.Append(Environment.NewLine);
                    msg.Append(ex.StackTrace);
                    msg.Append(Environment.NewLine);
                }

                if (ex.Source != null)
                {
                    msg.Append("Source:");
                    msg.Append(Environment.NewLine);
                    msg.Append(ex.Source);
                    msg.Append(Environment.NewLine);
                }

                if (ex.TargetSite != null)
                {
                    msg.Append("TargetSite:");
                    msg.Append(Environment.NewLine);
                    msg.Append(ex.TargetSite.ToString());
                    msg.Append(Environment.NewLine);
                }

                var baseException = ex.GetBaseException();
                if (baseException != null)
                {
                    msg.Append("BaseException:");
                    msg.Append(Environment.NewLine);
                    msg.Append(ex.GetBaseException());
                }

#pragma warning disable GCop435 // Finally block should not be empty
#pragma warning disable RECS0118 // Redundant empty finally block
            }
            finally
            {
#pragma warning restore RECS0118 // Redundant empty finally block
#pragma warning restore GCop435 // Finally block should not be empty
            }

            return msg.ToString();
        }
    }
}