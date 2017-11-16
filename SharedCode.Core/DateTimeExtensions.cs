// <copyright file="DateTimeExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;

    using JetBrains.Annotations;

    /// <summary>
    ///     The date time extensions class
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Adds the specified number of work days to the date.
        /// </summary>
        /// <param name="dto">The date time.</param>
        /// <param name="days">The number of work days to add.</param>
        /// <returns>The date time.</returns>
        public static DateTimeOffset AddWorkdays(this DateTimeOffset dto, int days)
        {
            // start from a weekday
            while (dto.DayOfWeek.IsWeekday())
            {
                dto = dto.AddDays(1.0);
            }

            for (var i = 0; i < days; ++i)
            {
                dto = dto.AddDays(1.0);
                while (dto.DayOfWeek.IsWeekday())
                {
                    dto = dto.AddDays(1.0);
                }
            }

            return dto;
        }

        /// <summary>
        ///     Returns age based on the specified date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The age.</returns>
        public static int Age(this DateTimeOffset dateOfBirth)
            => (DateTime.Today.Month < dateOfBirth.Month || DateTime.Today.Month == dateOfBirth.Month)
               && DateTime.Today.Day < dateOfBirth.Day
                ? DateTime.Today.Year - dateOfBirth.Year - 1
                : DateTime.Today.Year - dateOfBirth.Year;

        /// <summary>
        ///     Gets the date range between this date time and the specified date time.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">  The to date.</param>
        /// <returns>The date range <paramref name="fromDate" /> to <paramref name="toDate" />.</returns>
        [NotNull]
        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime fromDate, DateTime toDate)
        {
            Contract.Ensures(Contract.Result<IEnumerable<DateTime>>() != null);

            return Enumerable.Range(0, new TimeSpan(toDate.Ticks - fromDate.Ticks).Days)
                             .Select(p => fromDate.Date.AddDays(p));
        }

        /// <summary>
        ///     Gets the last day of the month.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The last day of the month.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);

        /// <summary>
        ///     Returns true if two date ranges intersect.
        /// </summary>
        /// <param name="startDate">            The start date.</param>
        /// <param name="endDate">              The end date.</param>
        /// <param name="intersectingStartDate">The intersecting start date.</param>
        /// <param name="intersectingEndDate">  The intersecting end date.</param>
        /// <returns><c>true</c> if two date ranges intersect, <c>false</c> otherwise.</returns>
        public static bool Intersects(
            this DateTime startDate,
            DateTime endDate,
            DateTime intersectingStartDate,
            DateTime intersectingEndDate)
            => intersectingEndDate >= startDate && intersectingStartDate <= endDate;

        /// <summary>
        /// Determines whether the specified date time is between the start and end dates.
        /// </summary>
        /// <param name="dt">The date time to check.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="compareTime">if set to <c>true</c> include the time in the comparison.</param>
        /// <returns><c>true</c> if the specified date time is between the start and end dates; otherwise, <c>false</c>.</returns>
        public static bool IsBetween(this DateTime dt, DateTime startDate, DateTime endDate, bool compareTime = false)
        {
            return compareTime
                ? dt >= startDate && dt <= endDate
                : dt.Date >= startDate.Date && dt.Date <= endDate.Date;
        }

        /// <summary>
        ///     Determines whether the specified value is weekend.
        /// </summary>
        /// <param name="value">The date value.</param>
        /// <returns><c>true</c> if the specified value is weekend; otherwise, <c>false</c>.</returns>
        public static bool IsWeekend(this DateTime value)
            => value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;

        /// <summary>
        ///     Converts the enumeration to the format string.
        /// </summary>
        /// <param name="source">        The source date time.</param>
        /// <param name="dateTimeFormat">The date time format.</param>
        /// <returns>The date time format string.</returns>
        [NotNull]
        public static string ToStringFormat(
            this DateTime source,
            [NotNull] Expression<Func<DateTimeFormat>> dateTimeFormat)
        {
            Contract.Requires(dateTimeFormat != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var dateTimeFormatCompiled = dateTimeFormat.Compile().Invoke();

            var dateTimeStringFormat = Enum<string>.GetStringValue(dateTimeFormatCompiled);

            var currentCulture = Thread.CurrentThread.CurrentCulture;

            return source.ToString(dateTimeStringFormat, currentCulture);
        }
    }
}