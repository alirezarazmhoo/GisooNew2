using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Utility
{
    public static class PersianCalendarDate
    {
        public static string PersianCalendarResult(DateTime d)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(d) + "/" + pc.GetMonth(d).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(d).ToString().PadLeft(2, '0');
        }
        public static DateTime MiladiCalendarResult(int year, int month, int day)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(year, month, day, pc);
            return dt;
        }
    }

}
public static class DateTimeHelper
{
    public static PersianDayOfWeek PersionDayOfWeek(this DateTime date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Saturday:
                return PersianDayOfWeek.Shanbe;
            case DayOfWeek.Sunday:
                return PersianDayOfWeek.Yekshanbe;
            case DayOfWeek.Monday:
                return PersianDayOfWeek.Doshanbe;
            case DayOfWeek.Tuesday:
                return PersianDayOfWeek.Seshanbe;
            case DayOfWeek.Wednesday:
                return PersianDayOfWeek.Charshanbe;
            case DayOfWeek.Thursday:
                return PersianDayOfWeek.Panjshanbe;
            case DayOfWeek.Friday:
                return PersianDayOfWeek.Jome;
            default:
                throw new Exception();
        }
    }
    public static string PersionDayOfWeekStr(this DateTime date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Saturday:
                return "شنبه";
            case DayOfWeek.Sunday:
                return "یکشنبه";
            case DayOfWeek.Monday:
                return "دوشنبه";
            case DayOfWeek.Tuesday:
                return "سه شنبه";
            case DayOfWeek.Wednesday:
                return "چهارشنبه";
            case DayOfWeek.Thursday:
                return "پنجشنبه";
            case DayOfWeek.Friday:
                return "جمعه";
            default:
                throw new Exception();
        }
    }
    public enum PersianDayOfWeek
    {
        Shanbe=1,
        Yekshanbe=2,
        Doshanbe=3,
        Seshanbe=4,
        Charshanbe=5,
        Panjshanbe=6,
        Jome=7
    }
}
