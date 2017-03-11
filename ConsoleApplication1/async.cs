using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class async
    {
        public static async Task<int> radom() {
            Random random = new Random();
            await Task.Run(()=> random.Next());
            return 1;
        }

    }
}
