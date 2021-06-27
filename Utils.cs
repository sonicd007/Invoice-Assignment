using System;
using System.Collections.Generic;
using System.Text;

namespace bread2
{
    public static class Utils
    {
        /*******************
        * Helper functions *
        *******************/

        /**
        Takes a DateTime object and returns a DateTime which is the first day
        of that month. For example:

        FirstDayOfMonth(new DateTime(2019, 2, 7)) // => new DateTime(2019, 2, 1)
        **/
        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /**
        Takes a DateTime object and returns a DateTime which is the last day
        of that month. For example:

        LastDayOfMonth(new DateTime(2019, 2, 7)) // => new DateTime(2019, 2, 28)
        **/
        public static DateTime LastDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static bool IsBetween(DateTime dateToTest, DateTime startDate, DateTime endDate)
        {
            return (dateToTest >= startDate && dateToTest <= endDate);
        }

        /**
        Takes a DateTime object and returns a DateTime which is the next day.
        For example:

        NextDay(new DateTime(2019, 2, 7))  // => new DateTime(2019, 2, 8)
        NextDay(new DateTime(2019, 2, 28)) // => new DateTime(2019, 3, 1)
        **/
        public static DateTime NextDay(DateTime date)
        {
            return date.AddDays(1);
        }
    }
}
