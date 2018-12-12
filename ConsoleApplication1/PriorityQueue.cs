using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /**
     * 用堆来实现优先队列
     * 堆(二叉堆):二叉堆是一个满足特定条件的完全二叉树，二叉堆分为最大堆和最小堆
     * 最大堆：任何一个父节点的值都大于或者等于他左右孩子节点的值
     * 最小堆：任何一个父节点的值都大于或者等于他左右孩子节点的值
     */

    /// <summary>
    /// 优先队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> where T :IComparable
    {
        private T[] queue;

        /// <summary>
        /// 元素个数
        /// </summary>
        private int size = 0;

        public int Length {
            get {
                return size;
            }
        }

        private Func<int, int> ConparaResult = r => r;
        //private int modCount = 0;

        //public PriorityQueue():this(16)
        //{

        //}
        private PriorityQueue(Func<int, int> conpararesult = null)
        {
            this.ConparaResult = conpararesult == null ? r => r : conpararesult;
        }
        private PriorityQueue(int initialCapacity, Func<int, int> conpararesult = null) :this(conpararesult)
        {
            this.queue = new T[initialCapacity];
            
        }

        private PriorityQueue(T[] array, Func<int, int> conpararesult = null) : this(conpararesult)
        {
            queue = array;
            size = array.Length;
            //构建堆
            BuildHeap();
        }

        //public PriorityQueue(IList<T> array, Func<int, int> conpararesult = null) : this(conpararesult)
        //{
        //    queue = array.ToArray();
        //    size = array.Count;
        //    //构建堆
        //    BuildHeap();
        //}

        /// <summary>
        /// 获取升序的优先队列，队列出队元素为最小元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueue()
        {
            return GetPriorityQueue(16);
        }
        /// <summary>
        /// 获取升序的优先队列，队列出队元素为最小元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueue(int initialCapacity)
        {
            PriorityQueue<T> result = new PriorityQueue<T>(initialCapacity);
            return result;
        }
        /// <summary>
        /// 获取升序的优先队列，队列出队元素为最小元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueue(T[] array)
        {
            PriorityQueue<T> result = new PriorityQueue<T>(array);
            return result;
        }
        /// <summary>
        /// 获取升序的优先队列，队列出队元素为最小元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueue(IList<T> array)
        {
            return GetPriorityQueue(array.ToArray());
        }
        /// <summary>
        /// 获取降序的优先队列，队列出队元素为最大元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueueDesc()
        {
            return GetPriorityQueueDesc(16);
        }
        /// <summary>
        /// 获取降序的优先队列，队列出队元素为最大元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueueDesc(int initialCapacity)
        {
            PriorityQueue<T> result = new PriorityQueue<T>(initialCapacity,r=>-r);
            return result;
        }
        /// <summary>
        /// 获取降序的优先队列，队列出队元素为最大元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueueDesc(T[] array)
        {
            PriorityQueue<T> result = new PriorityQueue<T>(array,r=>-r);
            return result;
        }
        /// <summary>
        /// 获取降序的优先队列，队列出队元素为最大元素
        /// </summary>
        /// <returns></returns>
        public static PriorityQueue<T> GetPriorityQueueDesc(IList<T> array)
        {
            return GetPriorityQueueDesc(array.ToArray());
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Enqueue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("入栈元素不能为空");
            }
            //modCount++;
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
                //将元素放在尾部，进行上浮操作
                queue[size-1] = item;
                UpAdjust();

            }
            return true;
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (size == 0)
            {
                return default(T);
            }
            //堆顶元素
            T result = queue[0];
            //把最末尾元素放在堆顶，然后进行下沉操作
            queue[0] = queue[size-1];
            queue[size-1] = default(T);
            size--;
            if (size != 0)
            {
                DownAdjust(0, size);
            }
            return result;
        }

        /// <summary>
        /// 上浮调整
        /// </summary>
        private void UpAdjust()
        {
            int childIndex = size - 1;
            int parentIndex = (childIndex - 1) / 2;
            T temp = queue[childIndex];
            while (childIndex > 0 && ConparaResult(temp.CompareTo(queue[parentIndex])) == -1)
            {
                queue[childIndex] = queue[parentIndex];
                childIndex = parentIndex;
                parentIndex = (parentIndex - 1) / 2;
            }
            queue[childIndex] = temp;
        }

        /// <summary>
        /// 下沉调整
        /// </summary>
        /// <param name="parentIndex"></param>
        /// <param name="length"></param>
        private void DownAdjust(int parentIndex, int length)
        {
            T temp = queue[parentIndex];
            //左孩子节点下标
            int childIndex = 2 * parentIndex + 1;
            while (childIndex < length)
            {
                if (childIndex + 1 < length && ConparaResult(queue[childIndex + 1].CompareTo(queue[childIndex])) == -1)
                {
                    childIndex++;
                }
                if (ConparaResult(temp.CompareTo(queue[childIndex])) != 1)
                {
                    break;
                }
                queue[parentIndex] = queue[childIndex];
                parentIndex = childIndex;
                childIndex = 2 * childIndex + 1;
            }
            queue[parentIndex] = temp;
        }

        /// <summary>
        /// 构造堆
        /// </summary>
        private void BuildHeap()
        {
            for (int i = size / 2; i >= 0; i--)
            {
                DownAdjust(i, size);
            }
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
