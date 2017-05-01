using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
namespace LogCollection
{
    public class SocketAsyncEventArgsPool
    {
        private ConcurrentStack<SocketAsyncEventArgs> _readWriteEventArg ;

        public SocketAsyncEventArgsPool(int num)
        {
            //num是用来初始化栈的大小的，现在先不用
            _readWriteEventArg = new ConcurrentStack<SocketAsyncEventArgs>();
        }

        public void Push(SocketAsyncEventArgs readWriteEventArg)
        {
            _readWriteEventArg.Push(readWriteEventArg);
        }

        public SocketAsyncEventArgs Pop()
        {
            SocketAsyncEventArgs result = null;
            _readWriteEventArg.TryPop(out result);
            return result;
        }
    }
}
