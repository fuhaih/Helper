using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using log4net;
using Quartz.Impl.Calendar;
namespace ConsoleApplication1.quartzTest
{
    public class example
    {
        public static ILog _log = LogManager.GetLogger(typeof(example));
        /// <summary>
        /// 用触发器来创建任务
        /// </summary>
        public static void TriggerRun()
        {
            _log.Info("正在初始化工作任务");
            
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                            .WithIdentity("job","group")
                            .Build();
            //ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
            //                        .WithIdentity("trigger", "group")
            //                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithMisfireHandlingInstructionIgnoreMisfires().RepeatForever())
            //                        .Build();
            _log.Info("正在设置触发器");
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                    .WithIdentity("trigger", "group")
                                    .WithCronSchedule("0 0/5 * * * ?")
                                    .Build();
            
            
            //WithCronSchedule:  秒 分钟 小时 日期（1~31） 月份（1~12） 星期几
            //*表示任意  -表示区间 

            _log.Info("注册工作任务");

            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler();
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
        }

        public static void XmlComfigRun()
        {
            // First we must get a reference to a scheduler
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // job initialization plugin handles our xml reading, without it defaults are used
            properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            properties["quartz.plugin.xml.fileNames"] = "quartz_jobs.xml";

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();
            TriggerKey key = new TriggerKey("simpleName", "simpleGroup");
            ITrigger triger= sched.GetTrigger(key);
            sched.Start();
        }
    }
}
