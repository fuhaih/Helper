using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace WebTest
{
    public class ServicePort
    {
        public static ServiceReferenceFactory.FactoryServiceClient Factory = new ServiceReferenceFactory.FactoryServiceClient();
        public static ServiceReferencePublic.PublicServiceClient Public = new ServiceReferencePublic.PublicServiceClient();

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

        private static Regex re = new Regex("^(-1|[0-9]*|[0-9]*.[0-9])/(-1|[0-9]*||[0-9]*.[0-9])$");

        public static Regex Re
        {
            get { return re; }
        }

        private static Regex reMD = new Regex("^[0-9]*|[0-9]*.[0-9]*$");

        public static Regex ReMD
        {
            get { return reMD; }
        }

        private static Regex isNum = new Regex("^[0-9]*|[0-9]*.[0-9]*$");

        public static Regex IsNum { get { return isNum; } }
    }
}