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


    }
}
