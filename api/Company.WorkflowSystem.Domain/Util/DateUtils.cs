using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Company.WorkflowSystem.Domain.Util
{
    public static class DateUtils
    {
        static TimeZoneInfo _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
        static CultureInfo _cultureInfo = CultureInfo.GetCultureInfo("en-NZ");

        public static DateTimeOffset GetDateTimeOffsetNow()
        {
            var now = DateTimeOffset.Now;
            return now.ToOffset(_timeZoneInfo.GetUtcOffset(now));

        }

        public static DateTimeOffset ToLocalTimeZone(this DateTimeOffset date)
        {
            var offset = _timeZoneInfo.GetUtcOffset(date);
            return date.ToOffset(offset);
        }

        public static DateTimeOffset LocalDateStringToLocalDate(this string dateString, string format)
        {
            var date = DateTime.SpecifyKind(DateTime.ParseExact(dateString, format, _cultureInfo), DateTimeKind.Unspecified);
            var offset = _timeZoneInfo.GetUtcOffset(DateTimeOffset.UtcNow);
            return new DateTimeOffset(date, offset);
        }

        public static string ToDateTimeWithTimeZoneString(this DateTimeOffset date)
        {
            var offset = _timeZoneInfo.GetUtcOffset(date);
            return date.ToString(_cultureInfo);
        }
        public static string ToDateString(this DateTimeOffset date)
        {
            var offset = _timeZoneInfo.GetUtcOffset(date);
            return date.ToString("d/M/yyyy", _cultureInfo);
        }


    }
}
