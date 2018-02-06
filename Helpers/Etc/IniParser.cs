using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
namespace Helpers.Etc
{
    public class IniParser
    {
        private string path = null;
        private int length = 255;
        public int MaxLength {
            get {
                return length;
            }
            set {
                length = value;
            }
        }
        [DllImport("kernel32")]//创建Section
        public static extern long WritePrivateProfileSection(string section, string val, string filePath);
        [DllImport("kernel32")] // 写入配置文件的接口
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")] // 读取配置文件的接口
        private static extern int GetPrivateProfileString(string section, string key, string def,StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]//获取所有Section名称
        private extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string filePath);
        [DllImport("kernel32")]//获取指定Section下的键值
        private static extern int GetPrivateProfileSection(string section, byte[] buffer, int nSize, string filePath);
        private IniParser(string path)
        {
            this.path = path;
        }
        public static IniParser Load(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            return new IniParser(path);   
        }
        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value,this.path);
        }
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        public void Delete(string section, string key)
        {
            WritePrivateProfileString(section, key, null, this.path);
        }
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <returns>string值</returns>
        public string ReadString(string section, string key)
        {
            StringBuilder sb = new StringBuilder(this.MaxLength);
            int success=GetPrivateProfileString(section, key, "", sb, this.MaxLength, this.path);
            return success==0?null:sb.ToString().Trim();
        }
        /// <summary>
        /// 获取所有的节点
        /// </summary>
        /// <returns></returns>
        public string[] GetSections()
        {
            byte[] buffer = new byte[this.MaxLength];
            int success= GetPrivateProfileSectionNamesA(buffer, this.MaxLength, this.path);
            string sections= Encoding.UTF8.GetString(buffer).Trim();
            Regex reg = new Regex("([^\0]+)");
            var matchs= reg.Matches(sections);
            string[] result = new string[matchs.Count];
            for (int i = 0; i < matchs.Count; i++)
            {
                result[i] = matchs[i].Value;
            }
            return success == 0 ? null : result;
        }
        /// <summary>
        /// 获取指定节点下的所有键值对
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Dictionary<string,string> GetKeyValues(string section)
        {
            byte[] buffer = new byte[this.MaxLength];
            int success = GetPrivateProfileSection(section, buffer, this.MaxLength, this.path);
            string values = Encoding.UTF8.GetString(buffer);
            Regex reg = new Regex("([^\0]+)");
            var matchs = reg.Matches(values);
            Dictionary<string, string> result = new Dictionary<string, string>();
            for (int i = 0; i < matchs.Count; i++)
            {
                string value = matchs[i].Value;
                int index= value.IndexOf("=");
                result.Add(value.Substring(0, index), value.Substring(index+1));
            }
            return success == 0 ? null : result;
        }
    }
}
