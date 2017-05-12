using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
namespace WebTest.Domain
{
    /// <summary>
    /// DomainHandler 的摘要说明
    /// </summary>
    public class DomainHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string actionName = context.Request.Form["Action"];
            if (actionName == null || actionName == "")
            {
                context.Response.End();
            }
            Type type = this.GetType();
            MethodInfo method = type.GetMethod(actionName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(this, new object[]{
                context
            });
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}