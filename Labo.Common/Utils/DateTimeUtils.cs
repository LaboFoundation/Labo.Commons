using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Labo.Common.Patterns;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Gets the month names.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static IList<MonthName> GetMonthNames(CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            return culture.DateTimeFormat.MonthNames
                .TakeWhile(m => !m.IsNullOrEmpty())
                .Select((m, i) => new MonthName
                    {
                        Number = i + 1,
                        Name = m
                    })
                .ToList();
        }

        /// <summary>
        /// Gets the first day of month.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(DateTime currentDate)
        {
            return new DateTime(currentDate.Year, currentDate.Month, 1);
        }

        /// <summary>
        /// Gets the last day of month.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(DateTime currentDate)
        {
            return GetFirstDayOfMonth(currentDate.AddMonths(1)).AddDays(-1).Date;
        }

        /// <summary>
        /// Gets the days of month.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static int GetDaysOfMonth(DateTime currentDate)
        {
            return GetLastDayOfMonth(currentDate).Day;
        }

        /// <summary>
        /// Gets the first day of week.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime currentDate)
        {
            return GetFirstDayOfWeek(currentDate, null);
        }

        /// <summary>
        /// Gets the first day of week.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime currentDate, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);
            return GetFirstDayOfWeek(currentDate, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        }

        /// <summary>
        /// Gets the first day of week.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="firstDay">The first day.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime currentDate, DayOfWeek firstDay)
        {
            return currentDate.AddDays(firstDay - currentDate.DayOfWeek).Date;
        }

        /// <summary>
        /// Gets the last day of week.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(DateTime currentDate)
        {
            return GetLastDayOfWeek(currentDate, null);
        }

        /// <summary>
        /// Gets the last day of week.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(DateTime currentDate, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);
            return GetLastDayOfWeek(currentDate, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        }

        /// <summary>
        /// Gets the last day of week.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="firstDay">The first day.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(DateTime current, DayOfWeek firstDay)
        {
            return GetFirstDayOfWeek(current, firstDay).AddDays(6);
        }

        /// <summary>
        /// Gets the midnight.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static DateTime GetMidnight(DateTime currentDate)
        {
            return currentDate.Date;
        }

        /// <summary>
        /// Gets the noon.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static DateTime GetNoon(DateTime currentDate)
        {
            return new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 0, 0);
        }

        /// <summary>
        /// Calculates the age.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        /// <returns></returns>
        public static int CalculateAge(DateTime dateOfBirth, IDateTimeProvider dateTimeProvider)
        {
            if (dateTimeProvider == null) throw new ArgumentNullException("dateTimeProvider");

            return CalculateAge(dateOfBirth, dateTimeProvider.Today);
        }

        /// <summary>
        /// Calculates the age.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns></returns>
        public static int CalculateAge(DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth, DateTimeProvider.Current);
        }

        /// <summary>
        /// Calculates the age.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public static int CalculateAge(DateTime dateOfBirth, DateTime currentDate)
        {
            int birthMonth = dateOfBirth.Month;
            int currentMonth = currentDate.Month;
            if (birthMonth < currentMonth || (birthMonth == currentMonth && dateOfBirth.Day <= currentDate.Day))
            {
                return currentDate.Year - dateOfBirth.Year;
            }

            return currentDate.Year - dateOfBirth.Year - 1;
        }
    }
}
