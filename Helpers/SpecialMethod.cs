using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    /// <summary>
    /// 奇技淫巧系列
    /// </summary>
    public class SpecialMethod
    {
        /// <summary>
        /// 判断奇偶，true为奇数，false为偶数
        /// </summary>
        /// <param name="Num">整数</param>
        /// <returns></returns>
        public bool JudgmentParity(int Num)
        {
            return (Num & 1) == 1;
        }
        /// <summary>
        /// 判断一个正整数是否是2的正整次幂
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public bool PositiveIntegerPower(int Num)
        {
            //2的幂都是大于0的，无论是正次幂还是负次幂
            //负次幂是小数，这里不考虑
            //原理
            //2的正整数次幂的二进制表示都是以1开头，然后后面都是0
            //比如：10000000
            //例子，a二进制为10000，则a-1二进制为01111,进行与操作为a&(a-1)=0
            return (Num & (Num - 1)) == 0 && (Num> 0);
        }
    }
}
