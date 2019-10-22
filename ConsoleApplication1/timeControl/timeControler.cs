using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
namespace ConsoleApplication1.timeControl
{
    /// <summary>
    /// 时间控制测试，主要是支路一场巡检中的时间控制比较乱
    /// 现在直接用quartz框架来配置异常巡检的时间
    /// 夜间巡检：每天早上七点进行，对前一天22:00至今6:00每15分钟进行巡检
    /// 夜间巡检表达式：0 0/15 0-5,22-23 * * ?
    /// 日渐巡检：早上6:00到晚上22:00,没小时一次 
    /// 日渐巡检表达式：0 0 6-21 * * ?
    /// </summary>
    class timeControler
    {   
        public static  void Test()
        {
            //夜间巡检时间
            DateTime endTime = DateTime.Parse("2017-06-01 07:00:00");
            DateTime startTime = endTime.AddDays(-1).AddSeconds(-1);


            //循环获取一天中的符合夜间巡检表达式的时间
            CronExpression expression = new CronExpression("0 0/15 0-5,22-23 * * ?");
            List<DateTime> nightTime = new List<DateTime>();
            while (true)
            {
                //bool bl = expression.IsSatisfiedBy(startTime);//判断时间是否符合表达式
                DateTimeOffset? timeoff = expression.GetNextValidTimeAfter(startTime);
                DateTime next = timeoff.Value.LocalDateTime;
                if(next >= endTime)
                {
                    break;
                }
                
                nightTime.Add(next);
                startTime = next;
            }
        }

        
    }
}
