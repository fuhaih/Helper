using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApplication1
{
    public class TaskTest
    {
        public void TestAttachedToParent()
        {
            Task task1 = new Task(() => {
                new Task(() => Console.WriteLine("sub1Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub2Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub3Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub4Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub5Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub6Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub7Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub8Task1执行结束")).Start();
                new Task(() => Console.WriteLine("sub9Task1执行结束")).Start();
            });

            task1.ContinueWith(task => Console.WriteLine("全部线程已结束"));
            task1.Start();
        }
    }
}
