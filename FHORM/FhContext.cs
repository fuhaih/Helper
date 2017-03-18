using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHORM
{
    public class FhContext
    {

    }

    public class FhTB<T> where T : FhContext
    {
        private string where_text = "";

        private string select_text = "";
    }
}
