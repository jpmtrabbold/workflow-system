using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Service.DataAggregators
{
    public class MonthsDurationWithDaysOffset
    {
        public int MonthsDuration { get; set; } = 0;
        public int DaysOffset { get; set; } = 0;

        public static (int monthsDuration, int daysOffset) GetMonthsDaysDuration(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var start = startDate.DateWithMinTime();
            var end = endDate.DateWithMinTime().AddDays(1); // this makes the day count to be inclusive, so 01/01/2020 - 31/01/2020 counts as 1 month.

            int monthsDuration = (12 * (end.Year - start.Year)) + end.Month - start.Month;
            int daysOffset = end.Day - start.Day;

            return (monthsDuration, daysOffset);
        }

        public static MonthsDurationWithDaysOffset FromDateDifference(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            (var monthsDuration, var daysOffset) = GetMonthsDaysDuration(startDate, endDate);
            return new MonthsDurationWithDaysOffset { MonthsDuration = monthsDuration, DaysOffset = daysOffset };
        }

        public void AddDatesOffset(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            (var monthsApart, var daysOffset) = GetMonthsDaysDuration(startDate, endDate);
            MonthsDuration += monthsApart;
            DaysOffset += daysOffset;

            // this is not exact (as we have months with 28, 29, 30 and 31), but is the best we can do in this particular case when we are not following a calendar.
            while (DaysOffset > 30)
            {
                MonthsDuration += 1;
                DaysOffset -= 30;
            }

            while (DaysOffset < -30)
            {
                MonthsDuration -= 1;
                DaysOffset += 30;
            }
        }

        public override string ToString()
        {
            if (DaysOffset == 0)
            {
                return MonthsDesc(MonthsDuration);
            }
            else
            {
                if (DaysOffset > 0)
                {
                    if (MonthsDuration == 0)
                        return $"{DaysDesc(DaysOffset)}";
                    else
                        return $"{MonthsDesc(MonthsDuration)} and {DaysDesc(DaysOffset)}";
                }
                else
                {
                    if ((MonthsDuration - 1) == 0)
                        return $"{30 + DaysOffset} days";
                    else
                        return $"{MonthsDesc(MonthsDuration - 1)} and {DaysDesc(30 + DaysOffset)}";
                }
            }
        }

        string MonthsDesc(int months) => $"{months} month{(months > 1 ? "s" : "")}";
        string DaysDesc(int days) => $"{days} day{(days > 1 ? "s" : "")}";

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        static int Comparison(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            if (months == monthsDurationWithDaysOffset.MonthsDuration)
            {
                if (monthsDurationWithDaysOffset.DaysOffset == 0)
                {
                    return 0;
                } 
                else if (monthsDurationWithDaysOffset.DaysOffset > 0)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (months > monthsDurationWithDaysOffset.MonthsDuration)
                    return 1;
                else
                    return -1;
            }
        }
        public override bool Equals(object obj)
        {

            if (!(obj is MonthsDurationWithDaysOffset)) return false;

            return MonthsDuration == ((MonthsDurationWithDaysOffset)obj).MonthsDuration &&
                DaysOffset == ((MonthsDurationWithDaysOffset)obj).DaysOffset;
        }


        public static bool operator <(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            return Comparison(months, monthsDurationWithDaysOffset) < 0;
        }

        public static bool operator <=(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            return Comparison(months, monthsDurationWithDaysOffset) <= 0;
        }

        public static bool operator >(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            return Comparison(months, monthsDurationWithDaysOffset) > 0;
        }
        public static bool operator >=(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            return Comparison(months, monthsDurationWithDaysOffset) >= 0;
        }

        public static bool operator ==(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            return Comparison(months, monthsDurationWithDaysOffset) == 0;
        }

        public static bool operator !=(int months, MonthsDurationWithDaysOffset monthsDurationWithDaysOffset)
        {
            return Comparison(months, monthsDurationWithDaysOffset) != 0;
        }


        public static bool operator <(MonthsDurationWithDaysOffset monthsDurationWithDaysOffset, int months)
        {
            return Comparison(months, monthsDurationWithDaysOffset) > 0;
        }
        public static bool operator <=(MonthsDurationWithDaysOffset monthsDurationWithDaysOffset, int months)
        {
            return Comparison(months, monthsDurationWithDaysOffset) >= 0;
        }
        public static bool operator >(MonthsDurationWithDaysOffset monthsDurationWithDaysOffset, int months)
        {
            return Comparison(months, monthsDurationWithDaysOffset) < 0;
        }
        public static bool operator >=(MonthsDurationWithDaysOffset monthsDurationWithDaysOffset, int months)
        {
            return Comparison(months, monthsDurationWithDaysOffset) <= 0;
        }
        public static bool operator ==(MonthsDurationWithDaysOffset monthsDurationWithDaysOffset, int months)
        {
            return Comparison(months, monthsDurationWithDaysOffset) == 0;
        }
        public static bool operator !=(MonthsDurationWithDaysOffset monthsDurationWithDaysOffset, int months)
        {
            return Comparison(months, monthsDurationWithDaysOffset) != 0;
        }


    }
}
