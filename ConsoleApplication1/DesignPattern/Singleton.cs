using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1.DesignPattern
{
    public class Singleton
    {
        private volatile static Singleton singleton=null;
        public int Count = 0;
        public static Singleton Instance {
            get {
                //直接用Interlocked的话，还是会每次都创建一个对象，虽然该对象没有赋值给singleton
                //Interlocked.CompareExchange<Singleton>(ref singleton, new Singleton(), null);
                //下面优化代码
                //和用锁实现相比，代码简洁了，不过也不会带来性能优势，
                //毕竟进入创建对象代码块的次数很少
                if (singleton == null)
                {
                    //volatile+cas 实现原子无锁同步
                    Interlocked.CompareExchange<Singleton>(ref singleton, new Singleton(), null);
                }
                return singleton;
            }
        }
        private Singleton()
        {
            
        }

        public void SayHello()
        {
            Interlocked.Increment(ref Count);
            Console.WriteLine("Hello");
        }
    }
}
