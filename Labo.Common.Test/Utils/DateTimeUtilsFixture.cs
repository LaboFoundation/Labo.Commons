using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Labo.Common.Patterns;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class DateTimeUtilsFixture
    {
        [Test, Sequential]
        public void FirstDayOfMonth([Values("10/10/2010", "4/12/2010", "2/12/2000", "11/6/2009")]DateTime current)
        {
            var result = DateTimeUtils.GetFirstDayOfMonth(current);

            Assert.AreEqual(0, result.Millisecond);
            Assert.AreEqual(0, result.Second);
            Assert.AreEqual(0, result.Minute);
            Assert.AreEqual(0, result.Hour);
            Assert.AreEqual(1, result.Day);
            Assert.AreEqual(current.Month, result.Month);
            Assert.AreEqual(current.Year, result.Year);
            Assert.True(result < current);
        }

        [Test, Sequential]
        public void LastDayOfMonth(
            [Values("10/10/2010", "4/12/2009", "2/11/2000", "3/3/1984", "2/2/2011")]string value,
            [Values("10/31/2010", "4/30/2009", "2/29/2000", "3/31/1984", "2/28/2011")]string expected)
        {
            DateTime date = DateTime.Parse(value, CultureInfo.InvariantCulture);
            DateTime expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            DateTime result = DateTimeUtils.GetLastDayOfMonth(date);

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void DaysOfMonth(
            [Values(31, 30, 29, 31, 28)]int expectedDays,
            [Values("10/10/2010", "4/12/2009", "2/11/2000", "3/3/1984", "2/2/2011")]string value)
        {
            var current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            var result = DateTimeUtils.GetDaysOfMonth(current);

            Assert.AreEqual(expectedDays, result);
        }

        [Test, Sequential]
        public void FirstDayOfWeek(
            [Values("3/21/2010", "4/11/2010","3/28/2010","3/22/2010","4/12/2010","3/29/2010")]string expected, 
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001","4/1/2010 5:23:15.001","3/25/2010 5:20:45.345","4/12/2010 5:23:15.001","4/1/2010 5:23:15.001")]string value, 
            [Values(DayOfWeek.Sunday, DayOfWeek.Sunday,DayOfWeek.Sunday,DayOfWeek.Monday,DayOfWeek.Monday,DayOfWeek.Monday)]DayOfWeek firstDay)
        {
            DateTime current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            DateTime expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            DateTime result = DateTimeUtils.GetFirstDayOfWeek(current, firstDay);

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void FirstDayOfWeekWithCulture(
            [Values("3/21/2010", "4/11/2010", "3/28/2010", "3/22/2010", "4/12/2010", "3/29/2010")]string expected,
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]string value,
            [Values("en-US", "en-US", "en-US", "tr-TR", "tr-TR", "tr-TR")]string cultureName)
        {
            DateTime current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            DateTime expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            DateTime result = DateTimeUtils.GetFirstDayOfWeek(current, CultureInfo.CreateSpecificCulture(cultureName));

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void LastDayOfWeek(
            [Values("3/27/2010", "4/17/2010", "4/3/2010", "3/28/2010", "4/18/2010", "4/4/2010")]string expected,
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]string value, 
            [Values(DayOfWeek.Sunday, DayOfWeek.Sunday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday)]DayOfWeek firstDay)
        {
            DateTime current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            DateTime expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            DateTime result = DateTimeUtils.GetLastDayOfWeek(current, firstDay);

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void LastDayOfWeekWithCulture(
            [Values("3/27/2010", "4/17/2010", "4/3/2010", "3/28/2010", "4/18/2010", "4/4/2010")]string expected,
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]string value,
            [Values("en-US", "en-US", "en-US", "tr-TR", "tr-TR", "tr-TR")]string cultureName)
        {
            DateTime current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            DateTime expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            DateTime result = DateTimeUtils.GetLastDayOfWeek(current, CultureInfo.CreateSpecificCulture(cultureName));

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void Midnight(
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]DateTime current)
        {
            DateTime result = DateTimeUtils.GetMidnight(current);

            Assert.AreEqual(current.Date, result);
        }

        [Test, Sequential]
        public void Noon(
             [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]DateTime current)
        {
            DateTime result = DateTimeUtils.GetNoon(current);

            Assert.AreEqual(0, result.Millisecond);
            Assert.AreEqual(0, result.Second);
            Assert.AreEqual(0, result.Minute);
            Assert.AreEqual(12, result.Hour);
            Assert.AreEqual(current.Day, result.Day);
            Assert.AreEqual(current.Month, result.Month);
            Assert.AreEqual(current.Year, result.Year);
        }

        [Test, Sequential]
        public void Age(
            [Values("3/27/2010", "3/27/2008", "3/27/2008", "3/3/1984")]string dateOfBirthStr,
            [Values("3/27/2010", "3/27/2010", "3/26/2010", "1/8/2012")]string todayStr,
            [Values(0, 2, 1, 27)]int expectedAge)
        {
            DateTime dateOfBirth = DateTime.Parse(dateOfBirthStr, CultureInfo.InvariantCulture);
            DateTime today = DateTime.Parse(todayStr, CultureInfo.InvariantCulture);
            int result = DateTimeUtils.CalculateAge(dateOfBirth, today);

            Assert.AreEqual(expectedAge, result);
        }

        [Test, Sequential]
        public void Age(
            [Values("3/27/2010", "3/27/2008", "3/27/2008", "3/3/1984")]string dateOfBirthStr)
        {
            DateTime dateOfBirth = DateTime.Parse(dateOfBirthStr, CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;
            using (new DateTimeProviderContext(new FuncDateTimeProvider(() => now)))
            {
                Assert.AreEqual(DateTimeUtils.CalculateAge(dateOfBirth, now), DateTimeUtils.CalculateAge(dateOfBirth));
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AgeWhenDateTimeProviderIsNullThrowsArgumentNullException()
        {
            DateTimeUtils.CalculateAge(new DateTime(2000, 9, 9), null);
        }

        private const string MONTH_NAMES_TR = "Ocak|1;Şubat|2;Mart|3;Nisan|4;Mayıs|5;Haziran|6;Temmuz|7;Ağustos|8;Eylül|9;Ekim|10;Kasım|11;Aralık|12";
        private const string MONTH_NAMES_EN = "January|1;February|2;March|3;April|4;May|5;June|6;July|7;August|8;September|9;October|10;November|11;December|12";

        [Test, Sequential]
        public void GetMonthNames(
            [Values(MONTH_NAMES_TR, MONTH_NAMES_EN)]string monthNames,
            [Values("tr-TR", "en-US")]string cultureName)
        {
            IList<MonthName> expected = ParseDateNames(monthNames, x => new MonthName
                {
                    Name = x[0],
                    Number = int.Parse(x[1])
                });
            Assert.AreEqual(expected, DateTimeUtils.GetMonthNames(new CultureInfo(cultureName)));
        }

        [Test, Sequential]
        public void GetMonthNamesThreadCulture(
            [Values(MONTH_NAMES_TR, MONTH_NAMES_EN)]string monthNames,
            [Values("tr-TR", "en-US")]string cultureName)
        {
            IList<MonthName> expected = ParseDateNames(monthNames, x => new MonthName
            {
                Name = x[0],
                Number = int.Parse(x[1])
            });
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Assert.AreEqual(expected, DateTimeUtils.GetMonthNames());
        }

        private const string DAY_NAMES_TR = "Pazar|7;Pazartesi|1;Salı|2;Çarşamba|3;Perşembe|4;Cuma|5;Cumartesi|6";
        private const string DAY_NAMES_EN = "Sunday|1;Monday|2;Tuesday|3;Wednesday|4;Thursday|5;Friday|6;Saturday|7";

        [Test, Sequential]
        public void GetDayNames(
            [Values(DAY_NAMES_TR, DAY_NAMES_EN)]string monthNames,
            [Values("tr-TR", "en-US")]string cultureName)
        {
            IList<DayName> expected = ParseDateNames(monthNames, x => new DayName
            {
                Name = x[0],
                Number = int.Parse(x[1])
            });
            Assert.AreEqual(expected, DateTimeUtils.GetDayNames(new CultureInfo(cultureName)));
        }

        [Test, Sequential]
        public void GetDayNamesThreadCulture(
            [Values(DAY_NAMES_TR, DAY_NAMES_EN)]string monthNames,
            [Values("tr-TR", "en-US")]string cultureName)
        {
            IList<DayName> expected = ParseDateNames(monthNames, x => new DayName
            {
                Name = x[0],
                Number = int.Parse(x[1])
            });
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Assert.AreEqual(expected, DateTimeUtils.GetDayNames());
        }

        private static IList<T> ParseDateNames<T>(string value, Func<string[], T> parser)
        {
            string[] monthNameStrings = value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<T> result = new List<T>(monthNameStrings.Length);
            for (int i = 0; i < monthNameStrings.Length; i++)
            {
                string monthNameString = monthNameStrings[i];
                string[] monthNameParts = monthNameString.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                result.Add(parser(monthNameParts));
            }
            return result;
        }
    }
}