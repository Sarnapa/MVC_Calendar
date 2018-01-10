using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MVC_Calendar.Controllers.Helpers
{
    public static class CalendarService
    {
        public static int GetWeekOfYear(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static int GetYear(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetYear(date);
        }

        public static DateTime GetFirstDateOfWeek(DateTime date)
        {
            // sunday
            if ((int)date.DayOfWeek == 0)
                return date.AddDays(-6);
            return date.AddDays(-(int)date.DayOfWeek + 1);
        }
    }
}