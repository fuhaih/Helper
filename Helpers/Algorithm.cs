using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class Algorithm
    {
        #region 求两个数最大公约数
        /// <summary>
        /// 求两个数最大公约数
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>返回最大公约数</returns>
        public static int getGreatestCommonDivisor(int numberA,int numberB)
        {
            int result = 1;
            if (numberA > numberB)
                result = gcd(numberA, numberB);
            else {
                result = gcd(numberB, numberA);
            }
            return result;
        }
        private static int gcd(int a,int b)
        {
            if (a == b)
            {
                return a;
            }
            if (a < b)
                return gcd(b, a);
            else {
                //(a & 1)==0则a是偶数
                if ((a & 1)==0&&(b&1)==0)
                {
                    return gcd(a >> 1, b >> 1)<<1;
                }
                else if ((a & 1) == 0)
                {
                    return gcd(a >> 1, b);
                }
                else if ((b & 1) == 0)
                {
                    return gcd(a, b >> 1);
                }
                else {
                    return gcd(b, a - b);
                }
            }

        }
        #endregion

        #region 回文数 manacher算法
        public static string Manacher(string s)
        {
            //预处理
            StringBuilder builder = new StringBuilder();
            builder.Append("#");
            for (int i = 0; i < s.Length; i++)
            {
                builder.Append("#" + s[i]);
            }
            builder.Append("#");
            string rs = builder.ToString();

            int[] RL = new int[rs.Length];
            int maxright = 0, pos = 0, maxlen = 0, maxpos = 0;

            for (int i = 0; i < rs.Length; i++)
            {
                if (i < maxright)
                {
                    RL[i] = Math.Min(RL[2 * pos - i], maxright - i);
                }
                else
                {
                    RL[i] = 1;
                }

                while (i - RL[i] > 0 && i + RL[i] < rs.Length && rs[i - RL[i]] == rs[i + RL[i]])
                {
                    RL[i]++;
                }

                if (RL[i] + i - 1 > maxright)
                {
                    maxright = RL[i] + i - 1;
                    pos = i;
                }
                if (maxlen < RL[i])
                {
                    maxlen = RL[i];
                    maxpos = i;
                }

            }
            int realstart = (maxpos - maxlen + 1) / 2;
            return s.Substring(realstart, maxlen - 1);
        }
        #endregion

    }
}
