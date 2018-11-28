using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class sort
    {
        /** 思路：两两比较，循环n(n-1)次
        */
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> BubbleSort<T>(this IList<T> list) where T : IComparable
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count-1; j++)
                {
                    if (list[j].CompareTo(list[j+1]) == 1)
                    {
                        T temp = list[j];
                        list[j] = list[j+1];
                        list[j+1] = temp;
                    }
                }
            }
            return list;
        }

        /**思路：从n个数中查找最小值下标，该下标的值和下标为0的值替换
        从剩下的n-1个数中查找最小值下标，该下标的值和下标为1的值替换
        以此类推
        */
        /// <summary>
        /// 选择排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> SelectSort<T>(this IList<T> list) where T : IComparable
        {
            for (int i = 0; i < list.Count()-1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < list.Count(); j++)
                {
                    //大于返回1
                    if (list[minIndex].CompareTo(list[j]) == -1)
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    T temp = list[i];
                    list[i] = list[minIndex];
                    list[minIndex] = temp;
                }
            }
            return list;
        }

        /**
         * 思路：从数组第二个数开始，往前进行插入操作，
         */
        /// <summary>
        /// 插入排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> InsertSort<T>(this IList<T> list) where T : IComparable
        {
            for (int i = 1; i < list.Count(); i++)
            {
                T temp = list[i];
                int j ;
                for (j = i; j >0&& temp.CompareTo(list[j-1]) == 1; j--)
                {
                    list[j] = list[j-1];
                }
                list[j] = temp;
            }
            return list;
        }

        /// <summary>
        /// 快速排序(左右指针法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">数组</param>
        /// <param name="left">排序起始位</param>
        /// <param name="right">排序结束位</param>
        /// <returns></returns>
        public static void QuickSort<T>(this IList<T> list,int left,int right) where T : IComparable
        {
            
            if (left > right)
            {
                return;
            }
            T mark = list[left], temp = list[left];
            int subleft = left, subright = right;
            while (subleft < subright)
            {
                //list[subright]>=mark
                while ((list[subright].CompareTo(mark) == 1 || list[subright].CompareTo(mark) == 0)&&subright>subleft)
                {
                    subright--;
                }
                //list[subleft]<=mark
                while ((list[subleft].CompareTo(mark) == -1 || list[subleft].CompareTo(mark) == 0)&&subleft<subright)
                {
                    subleft++;
                }
                if (subleft < subright)
                {
                    temp = list[subleft];
                    list[subleft] = list[subright];
                    list[subright] = temp;
                }
            }
            temp = list[subright];
            list[subright] = list[left];
            list[left] = temp;
            list.QuickSort(left, subright - 1);
            list.QuickSort(subright + 1, right);
        }

    }
}
