using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class DateExtensions
    {
        public static DateTimeOffset DateWithMinTime(this DateTimeOffset date) =>
            new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Offset);

        public static DateTimeOffset? DateWithMinTime(this DateTimeOffset? date)
        {
            if (date == null)
                return null;
            else
                return date.Value.DateWithMinTime();
        }

        public static DateTimeOffset DateWithMaxTime(this DateTimeOffset date) =>
            date.DateWithMinTime().AddDays(1).AddTicks(-1);

        public static DateTimeOffset? DateWithMaxTime(this DateTimeOffset? date)
        {
            if (date == null)
                return null;
            else
                return date.Value.DateWithMaxTime();
        }
    }
}
