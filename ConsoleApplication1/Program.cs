<<<<<<< Updated upstream
﻿using System;
using FHLog;
using System.Text;
using System.Threading;
using Polly;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
=======
﻿#define LOGGING
using System;
using FHLog;
using System.Text;
using System.Threading;
using Helpers.Etc;
using Polly;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Specialized;
using ConsoleApplication1.DesignPattern;
using System.Diagnostics;

>>>>>>> Stashed changes
namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static Latch latch = new Latch();
        static int count = 2;
        static CancellationTokenSource cts = new CancellationTokenSource();

        static void TestMethod()
        {
            while (!cts.IsCancellationRequested)
            {
                // Obtain the latch.
                if (latch.Wait(50))
                {
                    // Do the work. Here we vary the workload a slight amount
                    // to help cause varying spin counts in latch.
                    double d = 0;
                    if (count % 2 != 0)
                    {
                        d = Math.Sqrt(count);
                    }
                    count++;

                    // Release the latch.
                    latch.Set();
                }
            }
        }
        static void Main(string[] args)
        {
<<<<<<< Updated upstream
            int count = 0;
            int value = 0;
            
            Stack<int> stacks = new Stack<int>();
            bool bl= ReferenceEquals(count, value);
            Interlocked.CompareExchange(ref count, 1, 1);
            //List<Task> tasks = new List<Task>();
            //for (int i = 0; i < 10000; i++)
            //{
            //    Task task= Task.Factory.StartNew(()=> {
            //        Interlocked.CompareExchange(ref count,)
            //    });
            //    tasks.Add(task);
            //}
            //Task.WaitAll(tasks.ToArray());
            Console.WriteLine("".Equals(""));
            Console.ReadKey();
=======
            // Demonstrate latch with a simple scenario:
            // two threads updating a shared integer and
            // accessing a shared StringBuilder. Both operations
            // are relatively fast, which enables the latch to
            // demonstrate successful waits by spinning only. 

            latch.Set();


            // UI thread. Press 'c' to cancel the loop.
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Press 'c' to cancel.");
                if (Console.ReadKey().KeyChar == 'c')
                {
                    cts.Cancel();

                }
            });

            Parallel.Invoke(

                () => TestMethod(),
                () => TestMethod(),
                () => TestMethod()
                );

#if LOGGING
            latch.PrintLog();
#endif
            Console.WriteLine("\r\nPress the Enter Key.");
            Console.ReadLine();
>>>>>>> Stashed changes
        }
        public static Test Copy(Test test)
        {
            Test result = test;
            return result;
        }

        private static string Base64(byte[] buffer)
        {
            string[] base64 = new string[] {
                "A","B","C","D","E","F","G","H","I","J","K","L","M",
                "N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                "a","b","c","d","e","f","g","h","i","j","k","l","m",
                "n","o","p","q","r","s","t","u","v","w","x","y","z",
                "0","1","2","3","4","5","6","7","8","9","+","/"
            };
            //byte[] buffer = Encoding.Unicode.GetBytes(str);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(Convert.ToString(buffer[i], 2).PadLeft(8,'0'));
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < builder.Length; i = i + 6)
            {
                string charstr = builder.ToString(i, 6);
                result.Append(base64[Convert.ToInt32(charstr,2)]);
            }
            return result.ToString();
        }

    }
    class Latch
    {
        // 0 = unset, 1 = set
        private volatile int m_state = 0;

        private ManualResetEvent m_ev = new ManualResetEvent(false);

#if LOGGING
        // For fast logging with minimal impact on latch behavior.
        // Spin counts greater than 20 might be encountered depending on machine config.
        private int[] spinCountLog = new int[20];
        private volatile int totalKernelWaits = 0;

        public void PrintLog()
        {

            for (int i = 0; i < spinCountLog.Length; i++)
            {
                Console.WriteLine("Wait succeeded with spin count of {0} on {1} attempts", i, spinCountLog[i]);
            }
            Console.WriteLine("Wait used the kernel event on {0} attempts.", totalKernelWaits);
            Console.WriteLine("Logging complete");
        }
#endif

        public void Set()
        {
            // Trace.WriteLine("Set");
            m_state = 1;
            m_ev.Set();
        }

        public void Wait()
        {
<<<<<<< Updated upstream
            var plicy = Policy.Handle<Exception>().Retry(3, (ex, count, context) => {
                Console.WriteLine(string.Format("发生异常{0},尝试第{1}次", ex.Message, count));
            });
            Policy.Handle<Exception>();
            plicy.Execute(() => {
                Thread.Sleep(1000);
                throw new Exception("retry");
            });
=======
            Trace.WriteLine("Wait timeout infinite");
            Wait(Timeout.Infinite);
>>>>>>> Stashed changes
        }

        public bool Wait(int timeout)
        {
            // Allocated on the stack.
            SpinWait spinner = new SpinWait();
            Stopwatch watch;



<<<<<<< Updated upstream
    public class Test
    {
        public string name { get; set; }
        public Parent people { get; set;}
    }
    [XmlInclude(typeof(Children))]
    public class Parent
    {
        public string Name { get; set; }
    }

    public class Children : Parent
    {
        public string MyName { get; set;}
=======
            while (m_state == 0)
            {

                // Lazily allocate and start stopwatch to track timeout.
                watch = Stopwatch.StartNew();

                // Spin only until the SpinWait is ready
                // to initiate its own context switch.
                if (!spinner.NextSpinWillYield)
                {
                    spinner.SpinOnce();

                }
                // Rather than let SpinWait do a context switch now,
                //  we initiate the kernel Wait operation, because
                // we plan on doing this anyway.
                else
                {
                    totalKernelWaits++;
                    // Account for elapsed time.
                    int realTimeout = timeout - (int)watch.ElapsedMilliseconds;

                    // Do the wait.
                    if (realTimeout <= 0 || !m_ev.WaitOne(realTimeout))
                    {
                        Trace.WriteLine("wait timed out.");
                        return false;
                    }
                }
            }

            // Take the latch.
            m_state = 0;
            //   totalWaits++;

#if LOGGING
            spinCountLog[spinner.Count]++;
#endif


            return true;
        }
>>>>>>> Stashed changes
    }
}
