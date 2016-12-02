using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace ConsoleApplication1
{
    public class ServicePort
    {
        public static ServiceReferenceFactory.FactoryServiceClient Factory = new ServiceReferenceFactory.FactoryServiceClient();

        public static TTBEMS.Framework.UserKey Key
        {
            get
            {
                TTBEMS.Framework.UserKey uk = new TTBEMS.Framework.UserKey()
                {
                    key = TTBEMS.Framework.Security.RequestLicence(),
                    licence = "test-20140808151033",
                    userid = "0",
                    clientname = "matlab",
                    clientver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    desc = "",
                    dbsource = "310105"
                };
                return uk;
            }
        }

        public static string GetDescValue(string desc,string key)
        {
            Regex regex = new Regex(@"{" + key + ":.*?}");
            Match match = regex.Match(desc);
            if (match.Value == "")
            {
                return "";
            }
            string matchvalue = match.Value.Replace("{","").Replace("}","");
            return matchvalue.Split(':')[1];
        }


    }

}
