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
    }
}
