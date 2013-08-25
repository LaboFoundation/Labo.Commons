using System;
using System.Globalization;

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
            var current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            var expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            var result = DateTimeUtils.GetFirstDayOfWeek(current, firstDay);

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void LastDayOfWeek(
            [Values("3/27/2010", "4/17/2010", "4/3/2010", "3/28/2010", "4/18/2010", "4/4/2010")]string expected,
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]string value, 
            [Values(DayOfWeek.Sunday, DayOfWeek.Sunday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Monday)]DayOfWeek firstDay)
        {
            var current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            var expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            var result = DateTimeUtils.GetLastDayOfWeek(current, firstDay);

            Assert.AreEqual(expectedDate, result);
        }

        [Test, Sequential]
        public void Midnight(
            [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]DateTime current)
        {
            var result = DateTimeUtils.GetMidnight(current);

            Assert.AreEqual(current.Date, result);
        }

        [Test, Sequential]
        public void Noon(
             [Values("3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001", "3/25/2010 5:20:45.345", "4/12/2010 5:23:15.001", "4/1/2010 5:23:15.001")]DateTime current)
        {
            var result = DateTimeUtils.GetNoon(current);

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
            var result = DateTimeUtils.CalculateAge(dateOfBirth, today);

            Assert.AreEqual(expectedAge, result);
        }
    }
}