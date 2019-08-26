using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
namespace Helpers.DateTimeExtend
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取指定日期，在为一年中为第几周(一年中第一天为第一周)
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="firstDayOfWeek">指定一周的第一天</param>
        /// <returns>一年中的第几周</returns>
        public static int GetWeekOfYear(this DateTime dt, DayOfWeek firstDayOfWeek)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, firstDayOfWeek);
            return weekOfYear;
        }

        public static DateTime PreQuarter(this DateTime time)
        {
            int quarter = time.Minute / 15;
            time = time.Date.AddMinutes(quarter*15);
            return time;
        }

        public static DateTime NextQuarter(this DateTime time)
        {
            int quarter = time.Minute / 15;
            DateTime next = time.Date.AddMinutes((quarter+1) * 15);
            return time.AddMinutes(15) == next ? time : next;
        }
        /// <summary>
        /// 最近的刻钟 22.5分钟最近的刻钟是30分钟，22刻钟最近的刻钟是15分钟
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime NearQuarter(this DateTime time)
        {
            time = time.AddMinutes(7.5);
            time = time.Date.AddMinutes(time.Minute);
            return time;
        }
    }
}
