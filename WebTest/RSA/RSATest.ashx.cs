using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;
using WebTest.Domain;
using System.Security.Cryptography;
using System.Web.SessionState;
namespace WebTest.RSA
{
    /// <summary>
    /// RSATest 的摘要说明
    /// </summary>
    public class RSATest : DomainHandler, IRequiresSessionState
    {

        public void Login(HttpContext context)
        {
            string userName = context.Request.Form["username"];
            string password = context.Request.Form["passwd"];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            string privatekey = (string)context.Session["ttbem_privatekey"];
            byte[] pwdSecuri = HexStringToBytes(password);
            byte[] nameSecuri = HexStringToBytes(userName);
            rsa.FromXmlString(privatekey);
            byte[] pwds = rsa.Decrypt(HexStringToBytes(password), false);
            byte[] names = rsa.Decrypt(HexStringToBytes(userName), false);
            userName = Encoding.Unicode.GetString(names);
            password = Encoding.Unicode.GetString(pwds);
            var result = new
            {
                username = userName,
                passwd = password
            };
            context.Response.Write(result.ToJsJson());

        }

        private string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        private static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

        public void getRasPublicKey(HttpContext context)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            string privatekey = rsa.ToXmlString(true);
            context.Session.Add("ttbem_privatekey", privatekey);
            RSAParameters parameter = rsa.ExportParameters(true);
            string Modulus = BytesToHexString(parameter.Modulus);
            string Exponent = BytesToHexString(parameter.Exponent);
            var result = new
            {
                Modulus = Modulus,
                Exponent = Exponent
            };
            context.Response.Write(result.ToJsJson());
        }
    }
}