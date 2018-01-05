using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
namespace FramworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();
            GroupMatcher<TriggerKey> match = GroupMatcher<TriggerKey>.GroupEquals("");
            

        }
    }
}
