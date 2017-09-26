using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.DateTimeExtend
{
    /// <summary>
    /// 时间区间
    /// </summary>
    public class DateTimeRange
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        /// <summary>
        /// 区间分割
        /// </summary>
        /// <param name="fun">分割规则time=>time.AddDays(7)表示按7天分割</param>
        /// <returns></returns>
        public List<DateTimeRange> SplitRange(Func<DateTime, DateTime> fun)
        {
            List<DateTimeRange> result = new List<DateTimeRange>();
            DateTime start = this.Start;
            DateTime end = this.End;
            while (fun(start) <= end)
            {
                result.Add(new DateTimeRange
                {
                    Start = start,
                    End = fun(start)
                });
                start = fun(start);
            }
            if (start < end)
            {
                result.Add(new DateTimeRange
                {
                    Start = start,
                    End = end
                });
            }
            return result;
        }
    }
}
