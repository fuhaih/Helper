using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
namespace FHLog
{

    //在C#中，普通用锁很简单

    //    object m_lock = new object();
    //    lock(m_lock)
    //    {
    //        ......
    //    }
    //其中 ...... 表示互斥的代码。
    //这样就可以保证同时仅会有一个地方在执行这段互斥代码。

    //然而如果互斥代码中由await调用，上面的方式就行不通了，由于普通的lock代码段中无法存在await调用。

    //但是在实际使用中，经常遇见需要保护互斥的await情况，
    //比如用 await FileIO.WriteTextAsync() 的调用写文件，需要保证同时仅有一个地方在调用此段代码，不然就会出现互斥错误。

    //以下两篇文章很好的说明了如何实现对await调用互斥的处理。
    //http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266983.aspx
    //http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266988.aspx
    class AsyncSemaphore
    {
        private readonly static Task s_completed = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> m_waiters = new Queue<TaskCompletionSource<bool>>();
        private int m_currentCount;

        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0) throw new ArgumentOutOfRangeException("initialCount");
            m_currentCount = initialCount;
        }

        public Task WaitAsync()
        {
            lock (m_waiters)
            {
                if (m_currentCount > 0)
                {
                    --m_currentCount;
                    return s_completed;
                }
                else
                {
                    var waiter = new TaskCompletionSource<bool>();
                    m_waiters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (m_waiters)
            {
                if (m_waiters.Count > 0)
                    toRelease = m_waiters.Dequeue();
                else
                    ++m_currentCount;
            }
            if (toRelease != null)
                toRelease.SetResult(true);
        }
    }

    public class AsyncLock
    {
        private readonly AsyncSemaphore m_semaphore;
        private readonly Task<Releaser> m_releaser;

        public AsyncLock()
        {
            m_semaphore = new AsyncSemaphore(1);
            m_releaser = Task.FromResult(new Releaser(this));
        }

        public Task<Releaser> LockAsync()
        {
            var wait = m_semaphore.WaitAsync();
            return wait.IsCompleted ?
                m_releaser :
                wait.ContinueWith((_, state) => new Releaser((AsyncLock)state),
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public struct Releaser : IDisposable
        {
            private readonly AsyncLock m_toRelease;

            internal Releaser(AsyncLock toRelease) { m_toRelease = toRelease; }

            public void Dispose()
            {
                if (m_toRelease != null)
                    m_toRelease.m_semaphore.Release();
            }
        }
    }

    public class test
    {
        readonly AsyncLock m_lock = new AsyncLock();
        public async void WriteTextAsync()
        { 
        
            using (var releaser = await m_lock.LockAsync())
            {
                //await FileIO.WriteTextAsync(configureFile, jsonString);
            }
        }
    }
}
