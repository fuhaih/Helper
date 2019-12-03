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
        /// <summary>
        /// 上一个整n分钟，如果time已经是整n分钟，会返回当前时间。如 2019/10/01 12:15:00:000
        /// n只能是5,10,15,30，其他n无意义
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="timespan">时间间隔n => 5,10,15,30</param>
        /// <returns></returns>
        public static DateTime PreMinutes(this DateTime time, int timespan)
        {
            if (timespan <= 0 || timespan > 30 || timespan % 5 != 0)
            {
                throw new ArgumentOutOfRangeException("timespan");
            }
            int num = time.Minute / timespan;
            time = time.Date.AddHours(time.Hour).AddMinutes(num * timespan);
            return time;
        }
        /// <summary>
        /// 下一个整n分钟，如果time已经是整n分钟，会返回当前时间。如 2019/10/01 12:15:00:000
        /// n只能是5,10,15,30，其他n无意义
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="timespan">时间间隔n => 5,10,15,30</param>
        /// <returns></returns>
        public static DateTime NextMinutes(this DateTime time, int timespan)
        {
            if (timespan <= 0 || timespan > 30 || timespan % 5 != 0)
            {
                throw new ArgumentOutOfRangeException("timespan");
            }
            int num = time.Minute / timespan;
            DateTime next = time.Date.AddHours(time.Hour).AddMinutes((num + 1) * timespan);
            return time.AddMinutes(timespan) == next ? time : next;
        }
        /// <summary>
        /// 最近的整n分钟，如timespan为15 那么   minutes 大于等于7.5分 且 小于22.5分 为15分
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timespan">时间间隔</param>
        /// <returns></returns>
        public static DateTime NearMinutes(this DateTime time, int timespan)
        {
            //原理 15分钟为例
            //                 0              15              30                45
            //中间值                 7.5             22.5             37.5
            // 15-22.5最近的15分钟为15,22.5 - 30最近的15分钟为30
            // 转换一下，都加上7.5，就变成了找上一个15分钟了。
            if (timespan <= 0 || timespan > 30 || timespan % 5 != 0)
            {
                throw new ArgumentOutOfRangeException("timespan");
            }
            double minutes = timespan / 2d;
            time = time.AddMinutes(minutes);
            return time.PreMinutes(timespan);
        }
    }
}
