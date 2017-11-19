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
        /// DateDiff in SQL style.
        /// Datepart implemented:
        /// "year" (abbr. "yy", "yyyy"),
        /// "quarter" (abbr. "qq", "q"),
        /// "month" (abbr. "mm", "m"),
        /// "day" (abbr. "dd", "d"),
        /// "week" (abbr. "wk", "ww"),
        /// "hour" (abbr. "hh"),
        /// "minute" (abbr. "mi", "n"),
        /// "second" (abbr. "ss", "s"),
        /// "millisecond" (abbr. "ms").
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="datePart">The date part.</param>
        /// <param name="endDate">The end date.</param>
        /// <exception cref="Exception">The date part is unknown.</exception>
        /// <returns>The date difference.</returns>
        public static long DateDiff(this DateTime startDate, [NotNull] string datePart, DateTime endDate)
        {
            Contract.Requires(datePart != null);

            long dateDiffVal;
            var cal = Thread.CurrentThread.CurrentCulture.Calendar;
            var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (datePart.ToLower().Trim())
            {
                case "year":
                case "yy":
                case "yyyy":
                    dateDiffVal = cal.GetYear(endDate) - cal.GetYear(startDate);
                    break;

                case "quarter":
                case "qq":
                case "q":
                    dateDiffVal = (((cal.GetYear(endDate) - cal.GetYear(startDate)) * 4) + ((cal.GetMonth(endDate) - 1) / 3)) - ((cal.GetMonth(startDate) - 1) / 3);
                    break;

                case "month":
                case "mm":
                case "m":
                    dateDiffVal = ((cal.GetYear(endDate) - cal.GetYear(startDate)) * 12 + cal.GetMonth(endDate)) - cal.GetMonth(startDate);
                    break;

                case "day":
                case "d":
                case "dd":
                    dateDiffVal = (long)ts.TotalDays;
                    break;

                case "week":
                case "wk":
                case "ww":
                    dateDiffVal = (long)(ts.TotalDays / 7);
                    break;

                case "hour":
                case "hh":
                    dateDiffVal = (long)ts.TotalHours;
                    break;

                case "minute":
                case "mi":
                case "n":
                    dateDiffVal = (long)ts.TotalMinutes;
                    break;

                case "second":
                case "ss":
                case "s":
                    dateDiffVal = (long)ts.TotalSeconds;
                    break;

                case "millisecond":
                case "ms":
                    dateDiffVal = (long)ts.TotalMilliseconds;
                    break;

                default:
                    throw new Exception($"DatePart \"{datePart}\" is unknown");
            }

            return dateDiffVal;
        }

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

        /// <summary>
        /// Converts a System.DateTime object to Unix timestamp
        /// </summary>
        /// <param name="date">The date time.</param>
        /// <returns>The Unix timestamp.</returns>
        public static long ToUnixTimestamp(this DateTime date)
        {
            var unixEpoch = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0);
            var unixTimeSpan = date - unixEpoch;

            return (long)unixTimeSpan.TotalSeconds;
        }
    }
}