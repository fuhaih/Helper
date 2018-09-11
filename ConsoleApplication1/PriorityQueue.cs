using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class PriorityQueue<T> where T :IComparable
    {
        private T[] queue;
        private int size = 0;
        private int modCount = 0;

        public PriorityQueue():this(16)
        {

        }

        public PriorityQueue(int initialCapacity)
        {
            this.queue = new T[initialCapacity];
        }

        public bool Enqueue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("入栈元素不能为空");
            }
            modCount++;
            int i = size;
            if (i >= this.queue.Length)
            {
                //扩容方法
                Grow(i+1);
            }
            size++;

            if (i == 0)
            {
                queue[0] = item;
            }
            else {
                //将元素放在尾部，进行上移操作

            }
            return true;
        }

        /// <summary>
        /// 数组扩容
        /// </summary>
        /// <param name="minCapacity"></param>
        private void Grow(int minCapacity)
        {
            if (minCapacity < 0)
            {
                throw new ArgumentException("minCapacity不能小于零");
            }
            int oldCapaciy = queue.Length;
            int newCapacity = (oldCapaciy < 64) ? oldCapaciy * 2 : (oldCapaciy / 2) * 3;
            if (newCapacity < 0)
            {
                newCapacity = int.MaxValue;
            }
            if (newCapacity < minCapacity)
            {
                newCapacity = minCapacity;
            }
            T[] newqueue = new T[newCapacity];
            Array.Copy(queue, newqueue, queue.Length);
            queue = newqueue;
        }

    }
}
