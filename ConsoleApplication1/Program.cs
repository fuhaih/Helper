using System;
using FHLog;
using System.Text;
using System.Threading;
using Polly;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Helpers.Etc;
using System.Collections;
using System.Collections.Specialized;
using ConsoleApplication1.DesignPattern;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Threading;

namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {

            AsyncPump.Run(async delegate
            {

                await DemoAsync().ConfigureAwait(false);

            });
            Console.ReadKey();
            Regex reg = new Regex("\"\\w+\"\\s*:\\s*(\\w|\")+");
            reg.Replace("", match =>
            {
                return "";
            });

        }
        static async Task DemoAsync()
        {

            var d = new Dictionary<int, int>();

            for (int i = 0; i < 10000; i++)
            {

                int id = Thread.CurrentThread.ManagedThreadId;

                int count;

                d[id] = d.TryGetValue(id, out count) ? count + 1 : 1;

                await Task.Run(()=> { });

            }

            foreach (var pair in d) Console.WriteLine(pair);

        }
        public async Task test()
        {
            Task task = new Task(()=> { });
            await task;
            //MyTask task = new MyTask();
            //await task;

            //FuncTest test = new FuncTest();
            //await test;
            //IObservable<int> observable = Observable.Range(0, 3).Do(Console.WriteLine);
            //await observable;
            //int result = await new Func<int>(() => 0);
        }

        public async Task Test1()
        {
            await test();
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
    public interface IAwaitable<out TResult>
    {
        IAwaiter<TResult> GetAwaiter();
    }
    public interface IAwaiter<out TResult> : INotifyCompletion // or ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        TResult GetResult();
    }
    public interface IAwaitable
    {
        IAwaiter GetAwaiter();
    }
    public interface IAwaiter : INotifyCompletion // or ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }

    //public struct FuncAwaitable<TResult> : IAwaitable<TResult>
    //{
    //    private readonly Func<TResult> function;
    //    public FuncAwaitable(Func<TResult> function)
    //    {
    //        this.function = function;
    //    }
    //    public IAwaiter<TResult> GetAwaiter()
    //    {
    //        return new FuncAwaiter<TResult>(this.function);
    //    }
    //}

    public struct FuncAwaiter<TResult> : IAwaiter<TResult>
    {
        private readonly Task<TResult> task;

        public FuncAwaiter(Func<TResult> function)
        {
            this.task = new Task<TResult>(function);
            this.task.Start();
        }

        bool IAwaiter<TResult>.IsCompleted
        {
            get
            {
                return this.task.IsCompleted;
            }
        }

        TResult IAwaiter<TResult>.GetResult()
        {
            return this.task.Result;
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            new Task(continuation).Start();
        }
    }

    public struct FuncAwaiter : IAwaiter
    {
        private readonly Task task;
        public bool IsCompleted
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public FuncAwaiter(Action function)
        {
            this.task = new Task(function);
            this.task.Start();
        }
        public void GetResult()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }

    public static class FuncExtensions
    {
        public static IAwaiter<TResult> GetAwaiter<TResult>(this Func<TResult> function)
        {
            return new FuncAwaiter<TResult>(function);
        }
    }

    public class FuncTest
    {
        public IAwaiter GetAwaiter()
        {
            return new FuncAwaiter(()=> { });
        }
    }

    public class MyTask
    {
        public TaskAwaiter GetAwaiter()
        {
            Task task = new Task(()=> { });
            return task.GetAwaiter();
        }
    }
}
