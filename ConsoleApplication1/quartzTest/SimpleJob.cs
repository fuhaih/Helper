using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Quartz;
using log4net;
namespace ConsoleApplication1.quartzTest
{
    class SimpleJob:IJob
    {
        ILog _log = LogManager.GetLogger(typeof(SimpleJob));
        public virtual void Execute(IJobExecutionContext context)
        {
            DateTime time = context.FireTimeUtc.Value.LocalDateTime;
            DateTime nextTime = context.NextFireTimeUtc.Value.LocalDateTime;
            _log.Info("开始执行定时任务" + time.ToString());
            _log.Info("任务下次执行时间" + nextTime.ToString());

            _log.Info("每秒钟一次" + time.ToString());
            Thread.Sleep(15000);
        }
    }
    class SimpleJob1 : IJob
    {
        public virtual void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("每两秒钟一次" + DateTime.Now.ToString());
        }
    }

}
